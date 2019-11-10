using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class ImportMappedDataConfiguration : IEntityTypeConfiguration<ImportMappedDataModel>
  {
    public void Configure(EntityTypeBuilder<ImportMappedDataModel> builder)
    {
      builder.ToTable("UPLOAD_MAPPEDDATA");
      builder.HasKey(p => p.Id);

      builder.Property(p => p.Id).HasColumnName("ID").HasDefaultValue();
      builder.Property(p => p.UploadHistoryId).HasColumnName("UPLOADHISTORYID");
      builder.Property(p => p.ImportRawDataId).HasColumnName("UPLOADRAWDATAID");
      builder.Property(p => p.FileOperatorId).HasColumnName("FILE_TGOPERATORID");
      builder.Property(p => p.FileDirection).HasColumnName("FILE_DIRECTION");
      builder.Property(p => p.FileDateOpen).HasColumnName("FILE_VALIDFROM");
      builder.Property(p => p.FileDateClose).HasColumnName("FILE_VALIDUNTIL");
      builder.Property(p => p.LCROperatorId).HasColumnName("LCR_TGOPERATORID");
      builder.Property(p => p.LCRDirection).HasColumnName("LCR_DIRECTION");
      builder.Property(p => p.LCRDateOpen).HasColumnName("LCR_VALIDFROM");
      builder.Property(p => p.LCRDateClose).HasColumnName("LCR_VALIDUNTIL");
      builder.Property(p => p.Flags).HasColumnName("FLAGS");
      builder.Property(p => p.Approved).HasColumnName("APPROVED");
      builder.Property(p => p.Excluded).HasColumnName("EXCLUDED");

      
      builder.HasOne(p => p.UploadHistory).WithMany(p => p.ImportMappedData).HasForeignKey(p => p.UploadHistoryId);
      builder.HasOne(p => p.ImportRawData)
        .WithOne(p => p.ImportMappedData)
        .HasForeignKey<ImportMappedDataModel>(p => p.ImportRawDataId)
        ;
    }
  }
}
