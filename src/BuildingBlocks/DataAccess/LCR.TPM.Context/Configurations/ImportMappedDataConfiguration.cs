using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  class ImportMappedDataConfiguration : IEntityTypeConfiguration<ImportMappedDataModel>
  {
    public void Configure(EntityTypeBuilder<ImportMappedDataModel> builder)
    {
      builder.HasKey(p => p.Id);
    }
  }
}
