using LCR.DataService.Abstractions;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LCR.DataService
{
  public class DataService : IDataService
  {
    public DataService(TPMContext tpmCtx)
    {
      this.TPMContext = tpmCtx;
    }

    public TPMContext TPMContext { get; }

    public async Task<IEnumerable<SwitchModel>> Switch_GetListAsync()
    {
      var list = await this.TPMContext.Switches.ToListAsync();

      return list;
    }

    public async Task RawData_MapDataAsync(decimal uploadHistoryId)
    {
      var dataMappingQuery = @"declare
        l_res number;
        begin
          l_res:=lcr_tg_import_iapi.fill_upload_mappeddata(:uploadHistoryId);
        end;";

      using (var historySqlParam = new OracleParameter("uploadHistoryId", uploadHistoryId))
      {
        await this.TPMContext.Database.ExecuteSqlCommandAsync(dataMappingQuery, historySqlParam);
      }
    }

    public async Task MappedData_SetFlagsAsync(decimal uploadHistoryId)
    {
      var setFlagsQuery = @"
        UPDATE
          UPLOAD_MAPPEDDATA
        SET
          FLAGS = FLAGS - (BITAND(FLAGS, :flag)) + :flag
        WHERE
          UPLOADHISTORYID = :uploadHistoryId
      ";

      Func<int, string> getFlagPart = (int flag) =>
      {
        switch (flag)
        {
          case 2:
            return "LCR_TGID IS NULL";
          case 4:
            return "FILE_VALIDFROM != LCR_VALIDFROM";
          case 8:
            return "FILE_VALIDUNTIL != LCR_VALIDUNTIL";
          case 16:
            return "FILE_DIRECTION != LCR_DIRECTION";
          case 32:
            return "FILE_TGOPERATORID != LCR_TGOPERATORID";
          default:
            throw new Exception("Unknown flag");
        }

      };

      using (var historySqlParam = new OracleParameter("uploadHistoryId", uploadHistoryId))
      {
        var flags = new[] { 2, 4, 8, 16, 32 };
        foreach (var flag in flags)
        {
          var flagParam = new OracleParameter("flag", flag);

          var query = $"{setFlagsQuery} AND {getFlagPart(flag)}";

          await this.TPMContext.Database.ExecuteSqlCommandAsync(query, historySqlParam, flagParam);
        }
      }
    }


    public async Task MappedData_SetRowFlagsAsync(decimal uploadHistoryId, decimal rawDataId)
    {
      var setFlagsQuery = @"
        UPDATE
          UPLOAD_MAPPEDDATA
        SET
          FLAGS = FLAGS - (BITAND(FLAGS, :flag)) + :flag
        WHERE
          UPLOADHISTORYID = :uploadHistoryId
          AND
          UPLOADRAWDATAID = :uploadRawDataId
      ";

      Func<int, string> getFlagPart = (int flag) =>
      {
        switch (flag)
        {
          case 2:
            return "LCR_TGID IS NULL";
          case 4:
            return "FILE_VALIDFROM != LCR_VALIDFROM";
          case 8:
            return "FILE_VALIDUNTIL != LCR_VALIDUNTIL";
          case 16:
            return "FILE_DIRECTION != LCR_DIRECTION";
          case 32:
            return "FILE_TGOPERATORID != LCR_TGOPERATORID";
          default:
            throw new Exception("Unknown flag");
        }

      };

      using (var historySqlParam = new OracleParameter("uploadHistoryId", uploadHistoryId))
      {
        using (var rowSqlParam = new OracleParameter("uploadRawDataId", rawDataId))
        {
          var flags = new[] { 2, 4, 8, 16, 32 };
          foreach (var flag in flags)
          {
            var flagParam = new OracleParameter("flag", flag);

            var query = $"{setFlagsQuery} AND {getFlagPart(flag)}";

            await this.TPMContext.Database.ExecuteSqlCommandAsync(query, historySqlParam, rowSqlParam, flagParam);
          }
        }
      }
    }

    public async Task MappedData_DetectUnchangedRowsAsync(decimal uploadHistoryId, decimal previousHploadHistoryId)
    {
      var query = @"
        MERGE INTO
	        UPLOAD_MAPPEDDATA trg
        USING
	        (
		        SELECT
			        cumd.ID
		        FROM
			        (
				        SELECT
					        r.DARAROWID,
					        r.TRUNKGROUPNAME,
					        c.*
				        FROM
					        UPLOAD_MAPPEDDATA c
					        INNER JOIN
						        UPLOAD_RAWDATA r
						        ON
							        c.UPLOADRAWDATAID = r.ID
				        WHERE
					        c.UPLOADHISTORYID = :uploadHistoryId
			        ) cumd
			        INNER JOIN
				        (
					        SELECT
						        r.DARAROWID,
						        r.TRUNKGROUPNAME,
						        c.*
					        FROM
						        UPLOAD_MAPPEDDATA c
						        INNER JOIN
							        UPLOAD_RAWDATA r
						        ON
							        c.UPLOADRAWDATAID = r.ID
					        WHERE
						        c.UPLOADHISTORYID = :previousUploadHistoryId
				        ) pumd
				        ON
					        cumd.DARAROWID = pumd.DARAROWID
					        AND
					        cumd.TRUNKGROUPNAME = pumd.TRUNKGROUPNAME
		        WHERE
			        pumd.APPROVED = 1
			        OR
			        (
				        pumd.EXCLUDED = 0
				        AND
				        pumd.FLAGS = 64
			        )
			        AND
			        (
				        cumd.FILE_TGOPERATORID != pumd.FILE_TGOPERATORID
				        OR
				        cumd.FILE_DIRECTION != pumd.FILE_DIRECTION
				        OR
				        cumd.FILE_VALIDFROM != pumd.FILE_VALIDFROM
				        OR
				        cumd.FILE_VALIDUNTIL != pumd.FILE_VALIDUNTIL
			        )
	        ) src
        ON
	        (
		        trg.ID = src.ID
	        )
        WHEN 
	        MATCHED 
	        THEN
		        UPDATE
		        SET
			        FLAGS = 64
      ";

      using (var historySqlParam = new OracleParameter("uploadHistoryId", uploadHistoryId))
      {
        using (var prevHistorySqlParam = new OracleParameter("previousUploadHistoryId", previousHploadHistoryId))
        {
          await this.TPMContext.Database.ExecuteSqlCommandAsync(query, historySqlParam, prevHistorySqlParam);
        }
      }
    }


    public async Task Import_SaveAsync(decimal uploadHistoryId)
    {
      var saveQuery = @"declare
        l_res number;
        begin
          l_res:=lcr_tg_import_iapi.import_tg(:uploadHistoryId);
        end;";

      using (var historySqlParam = new OracleParameter("uploadHistoryId", uploadHistoryId))
      {
        var historyEntry = await this.TPMContext.UploadHistory.FindAsync(uploadHistoryId);
        if (historyEntry == null)
        {
          throw new ArgumentNullException(nameof(historyEntry), "Upload history was not found");
        }

        await this.TPMContext.Database.ExecuteSqlCommandAsync(saveQuery, historySqlParam);

        historyEntry.Step = ImportStep.Saved;

        await this.TPMContext.SaveChangesAsync();
      }
    }

    public async Task<IEnumerable<OperatorModel>> Operator_GetListAsync()
    {
      return await this.TPMContext.Operators.ToListAsync();
    }

    public async Task<IEnumerable<UserQueryModel>> User_GetListAsync()
    {
      return await this.TPMContext.Users.ToListAsync();
    }
  }
}
