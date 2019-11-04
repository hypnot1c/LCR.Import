using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class UploadHistoryConfiguration : IEntityTypeConfiguration<UploadHistoryModel>
  {
    public void Configure(EntityTypeBuilder<UploadHistoryModel> builder)
    {
      builder.ToTable("UPLOAD_HISTORY");
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Id).HasColumnName("ID").HasDefaultValue();
      builder.Property(p => p.UserId).HasColumnName("USERID");
      builder.Property(p => p.SwitchId).HasColumnName("SWITCHID");
      builder.Property(p => p.DateUpload).HasColumnName("DATEUPLOAD");
      builder.Property(p => p.FileName).HasColumnName("FILENAME");
      builder.Property(p => p.RowsAffected).HasColumnName("ROWSAFFECTED");
      builder.Property(p => p.Step).HasColumnName("STEP");
    }
  }
}
