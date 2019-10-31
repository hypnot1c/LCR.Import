using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  class ImportHistoryConfiguration : IEntityTypeConfiguration<ImportHistoryModel>
  {
    public void Configure(EntityTypeBuilder<ImportHistoryModel> builder)
    {
      //throw new System.NotImplementedException();
    }
  }
}
