using ExcelDataReader;
using LCR.Import.DataValidation;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommandService : IProccessFileCommandService<ProccessFileCommand>
  {
    public ProccessFileCommandService(
      TPMContext tpmCtx,
      ILogger<ProccessFileCommandService> logger
      )
    {
      this.TPMContext = tpmCtx;
      this.Logger = logger;
    }

    public TPMContext TPMContext { get; }
    public ILogger<ProccessFileCommandService> Logger { get; }

    public async Task OnNextAsync(ProccessFileCommand command)
    {
      var userId = command.UserId;
      var filePath = command.FilePath;

      using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          while (!reader.IsClosed && reader.Name != "Направления")
          {
            reader.NextResult();
          }
          do
          {
            var validator = new ImportDataValidator();
            while (reader.Read())
            {
              var firstValue = reader.GetValue(0);
              if (firstValue == null || !Int32.TryParse(firstValue.ToString(), out var number))
              {
                continue;
              }

              var rawData = reader.ToImportRawDataModel();
              rawData.UploadHistoryId = command.ImportHistoryId;

              var result = validator.Validate(rawData);

              if (!result.IsValid)
              {
                var errorCodes = result.Errors
                  .Select(vf => Int32.Parse(vf.ErrorCode))
                  .Aggregate((x, y) => x | y)
                  ;

                this.Logger.LogError("Data row {0} has validation errors. Codes {1}", rawData.DataRowId, errorCodes);

                var formatErrors = new ImportFormatErrorsModel();
                formatErrors.ImportRawData = rawData;
                formatErrors.ErrorFlags = errorCodes;
                this.TPMContext.ImportFormatErrors.Add(formatErrors);
              }
              else
              {
                var mappedData = rawData.ToMappedDataModel();
                this.TPMContext.ImportRawData.Add(rawData);
                //await this.TPMContext.SaveChangesAsync();
                this.TPMContext.ImportMappedData.Add(mappedData);
                //await this.TPMContext.SaveChangesAsync();
              }
            }
          } while (reader.NextResult());
        }
      }

      var historyEntry = await this.TPMContext.UploadHistory.FindAsync(command.ImportHistoryId);
      historyEntry.Step = TPM.Model.ImportStep.Applied;

      await this.TPMContext.SaveChangesAsync();

      var typed = await this.TPMContext.ImportMappedData.ToListAsync();
    }
  }
}
