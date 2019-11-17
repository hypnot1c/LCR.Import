using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class ImportResultConfiguration : IQueryTypeConfiguration<ImportResultQueryModel>
  {
    public ImportResultConfiguration(TPMContext ctx)
    {
      this.DbContext = ctx;
    }

    public TPMContext DbContext { get; }

    public void Configure(QueryTypeBuilder<ImportResultQueryModel> builder)
    {
      builder.Property(p => p.Id).HasColumnName("ID");
      builder.Property(p => p.DataRowId).HasColumnName("DARAROWID");
      builder.Property(p => p.UploadHistoryId).HasColumnName("UPLOADHISTORYID");
      builder.Property(p => p.ChannelBundleName).HasColumnName("TRUNKGROUPNAME");
      builder.Property(p => p.DirectionType).HasColumnName("TRUNKGROUPTYPE");
      builder.Property(p => p.Direction).HasColumnName("DIRECTION");
      builder.Property(p => p.OperatorsNetworkConnectionLvl).HasColumnName("OPERATORSNETWORKCONNECTLVL");
      builder.Property(p => p.PairedSwitchOperatorFullName).HasColumnName("PAIREDSWITCHOPERATORFULLNAME");
      builder.Property(p => p.RTNetworkConnectionLevel).HasColumnName("RTNETWORKCONNECTIONLEVEL");
      builder.Property(p => p.SwitchOperatorName).HasColumnName("SWITCHOPERATORNAME");
      builder.Property(p => p.ValidFrom).HasColumnName("VALIDFROM");
      builder.Property(p => p.ValidUntil).HasColumnName("VALIDUNTIL");
      builder.Property(p => p.FormatFlags).HasColumnName("FORMATFLAGS");
      builder.Property(p => p.LogicFlags).HasColumnName("LOGICFLAGS");
      builder.Property(p => p.FileOperatorId).HasColumnName("FILE_TGOPERATORID");
      builder.Property(p => p.FileDirection).HasColumnName("FILE_DIRECTION");
      builder.Property(p => p.FileDateOpen).HasColumnName("FILE_VALIDFROM");
      builder.Property(p => p.FileDateClose).HasColumnName("FILE_VALIDUNTIL");
      builder.Property(p => p.FileOperatorName).HasColumnName("FILE_OPERATORNAME");
      builder.Property(p => p.LCRTGID).HasColumnName("LCR_TGID");
      builder.Property(p => p.LCROperatorId).HasColumnName("LCR_TGOPERATORID");
      builder.Property(p => p.LCRDirection).HasColumnName("LCR_DIRECTION");
      builder.Property(p => p.LCRDateOpen).HasColumnName("LCR_VALIDFROM");
      builder.Property(p => p.LCRDateClose).HasColumnName("LCR_VALIDUNTIL");
      builder.Property(p => p.LCROperatorName).HasColumnName("LCR_OPERATORNAME");
      builder.Property(p => p.Approved).HasColumnName("APPROVED");
      builder.Property(p => p.Excluded).HasColumnName("EXCLUDED");

      builder.ToQuery(() => this.DbContext.ImportResults
        .FromSql(@"
          SELECT
	          urd.ID,
            urd.DARAROWID,
	          urd.UPLOADHISTORYID,
	          urd.TRUNKGROUPNAME,
	          urd.TRUNKGROUPTYPE,
            urd.DIRECTION,
	          urd.OPERATORSNETWORKCONNECTLVL,
	          urd.PAIREDSWITCHOPERATORFULLNAME,
	          urd.RTNETWORKCONNECTIONLEVEL,
	          urd.SWITCHOPERATORNAME,
            urd.VALIDFROM,
            urd.VALIDUNTIL,
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
        "));
    }
  }
}
