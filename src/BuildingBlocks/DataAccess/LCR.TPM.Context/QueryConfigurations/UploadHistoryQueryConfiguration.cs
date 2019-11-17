using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.QueryConfigurations
{
  public class UploadHistoryQueryConfiguration : IQueryTypeConfiguration<UploadHistoryQueryModel>
  {
    public UploadHistoryQueryConfiguration(TPMContext ctx)
    {
      this.DbContext = ctx;
    }

    public TPMContext DbContext { get; }

    public void Configure(QueryTypeBuilder<UploadHistoryQueryModel> builder)
    {
      builder.Property(p => p.Id).HasColumnName("ID").HasDefaultValue();
      builder.Property(p => p.UserId).HasColumnName("USERID");
      builder.Property(p => p.Firstname).HasColumnName("FIRSTNAME");
      builder.Property(p => p.Lastname).HasColumnName("LASTNAME");
      builder.Property(p => p.SwitchId).HasColumnName("SWITCHID");
      builder.Property(p => p.SwitchName).HasColumnName("SWITCHNAME");
      builder.Property(p => p.DateUpload).HasColumnName("DATEUPLOAD");
      builder.Property(p => p.FileName).HasColumnName("FILENAME");
      builder.Property(p => p.RowsAffected).HasColumnName("ROWSAFFECTED");
      builder.Property(p => p.Step).HasColumnName("STEP");

      builder.ToQuery(() => this.DbContext.UploadHistoryResults.FromSql(@"
        SELECT
          uh.ID,
          uh.USERID,
          u.FIRSTNAME,
          u.LASTNAME,
          uh.SWITCHID,
          s.Name as SWITCHNAME,
          uh.DATEUPLOAD,
          uh.FILENAME,
          uh.ROWSAFFECTED,
          uh.STEP
        FROM
          UPLOAD_HISTORY uh
          INNER JOIN
            (select DISTINCT Id, first_name as Firstname, last_name as Lastname from table(lcr_protocol_iapi.get_user_lst(0))) u
            ON
              uh.UserId = u.Id
          INNER JOIN
            (select DISTINCT Id, Name from table(lcr_tg_import_iapi.get_switch_lst)) s
            ON
              uh.SwitchId = s.Id
      "));
    }
  }
}
