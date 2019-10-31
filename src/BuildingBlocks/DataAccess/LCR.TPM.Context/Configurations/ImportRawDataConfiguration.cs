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
      //throw new System.NotImplementedException();
    }
  }
}
