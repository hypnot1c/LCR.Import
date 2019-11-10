using DataBaseContextExtensions;
using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LCR.TPM.Context
{
  public class TPMContext : DbContext
  {
    public TPMContext(DbContextOptions<TPMContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.UseEntityTypeConfiguration(typeof(TPMContext).GetTypeInfo().Assembly);
      modelBuilder.UseQueryTypeConfiguration(typeof(TPMContext).GetTypeInfo().Assembly);

      modelBuilder.Query<SwitchModel>().ToQuery(() => this.Switches
      .FromSql("select DISTINCT Id,Name from table(lcr_tg_import_iapi.get_switch_lst)")
      );

      modelBuilder.Query<OperatorModel>()
        .ToQuery(() => this.Operators
          .FromSql("select DISTINCT id,supplier_name as Name from table(lcr_tg_import_iapi.get_carrier_lst)")
          );

      modelBuilder.Query<ImportResultQueryModel>()
        .ToQuery(() => this.ImportResults
          .FromSql(@"
            SELECT
	            urd.ID,
              urd.DARAROWID,
	            urd.UPLOADHISTORYID,
	            urd.TRUNKGROUPNAME,
	            urd.TRUNKGROUPTYPE,
	            urd.OPERATORSNETWORKCONNECTLVL,
	            urd.PAIREDSWITCHOPERATORFULLNAME,
	            urd.RTNETWORKCONNECTIONLEVEL,
	            urd.SWITCHOPERATORNAME,
	            ufe.ERRORFLAGS as FORMATFLAGS,
	            umd.Flags as LOGICFLAGS,
	            umd.FILE_TGOPERATORID,
	            umd.FILE_DIRECTION,
	            umd.FILE_VALIDFROM,
	            umd.FILE_VALIDUNTIL,
	            fop.NAME as FILE_OPERATORNAME,
	            umd.LCR_TGID,
	            umd.LCR_TGOPERATORID,
	            umd.LCR_DIRECTION,
	            umd.LCR_VALIDFROM,
	            umd.LCR_VALIDUNTIL,
	            lop.NAME as LCR_OPERATORNAME,
	            umd.APPROVED,
	            umd.EXCLUDED
            FROM
	            UPLOAD_RAWDATA urd
	            LEFT JOIN
		            UPLOAD_FORMATERRORS ufe
		            ON
			            urd.ID = ufe.UPLOADRAWDATAID
	            LEFT JOIN
		            UPLOAD_MAPPEDDATA umd
		            ON
			            urd.ID = umd.UPLOADRAWDATAID
	            LEFT JOIN
		            (select DISTINCT ID, supplier_name as NAME from table(lcr_tg_import_iapi.get_carrier_lst)) fop
		            ON
			            umd.FILE_TGOPERATORID = fop.ID
	            LEFT JOIN
		            (select DISTINCT ID, supplier_name as NAME from table(lcr_tg_import_iapi.get_carrier_lst)) lop
		            ON
			            umd.LCR_TGOPERATORID = lop.ID
")
        );

    }

    public DbSet<UploadHistoryModel> UploadHistory { get; set; }
    public DbSet<ImportRawDataModel> ImportRawData { get; set; }
    public DbSet<ImportFormatErrorsModel> ImportFormatErrors { get; set; }
    public DbSet<ImportMappedDataModel> ImportMappedData { get; set; }
    public DbQuery<SwitchModel> Switches { get; set; }
    public DbQuery<OperatorModel> Operators { get; set; }
    public DbQuery<ImportResultQueryModel> ImportResults { get; set; }
  }
}
