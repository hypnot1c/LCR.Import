using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  class UploadHistoryConfiguration : IEntityTypeConfiguration<UploadHistoryModel>
  {
    public void Configure(EntityTypeBuilder<UploadHistoryModel> builder)
    {
      builder.HasKey(p => p.Id);
    }
  }
}
