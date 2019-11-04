using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class ImportFormatErrorsConfiguration : IEntityTypeConfiguration<ImportFormatErrorsModel>
  {
    public void Configure(EntityTypeBuilder<ImportFormatErrorsModel> builder)
    {
      builder.ToTable("UPLOAD_FORMATERRORS");
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Id).HasColumnName("ID").HasDefaultValue();
      builder.Property(p => p.ImportRawDataId).HasColumnName("UPLOADRAWDATAID");
      builder.Property(p => p.ErrorFlags).HasColumnName("ERRORFLAGS");

      //builder.HasOne(p => p.ImportRawData).WithOne(p => p.ImportFormatErrors).HasForeignKey<ImportFormatErrorsModel>(p => p.ImportRawDataId);
    }
  }
}
