using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  class ImportRawDataConfiguration : IEntityTypeConfiguration<ImportRawDataModel>
  {
    public void Configure(EntityTypeBuilder<ImportRawDataModel> builder)
    {
      builder.HasKey(p => p.Id);
      builder.HasOne(p => p.UploadHistory).WithMany(p => p.ImportRawData).HasForeignKey(p => p.UploadHistoryId);
      builder.HasOne(p => p.ImportMappedData).WithOne(p => p.ImportRawData).HasPrincipalKey<ImportMappedDataModel>(p => p.ImportRawDataId);
    }
  }
}
