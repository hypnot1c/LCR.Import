using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class OperatorConfiguration : IQueryTypeConfiguration<OperatorModel>
  {
    public OperatorConfiguration(TPMContext ctx)
    {
      this.DbContext = ctx;
    }

    public TPMContext DbContext { get; }

    public void Configure(QueryTypeBuilder<OperatorModel> builder)
    {
      builder.Property(p => p.Id).HasColumnName("ID");
      builder.Property(p => p.Name).HasColumnName("NAME");

      builder.ToQuery(() => this.DbContext.Operators
        .FromSql("select DISTINCT id,supplier_name as Name from table(lcr_tg_import_iapi.get_carrier_lst)")
        );
    }
  }
}
