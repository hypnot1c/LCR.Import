using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.QueryConfigurations
{
  public class SwitchConfiguration : IQueryTypeConfiguration<SwitchModel>
  {
    public SwitchConfiguration(TPMContext ctx)
    {
      this.DbContext = ctx;
    }

    public TPMContext DbContext { get; }

    public void Configure(QueryTypeBuilder<SwitchModel> builder)
    {
      builder.ToQuery(() => this.DbContext.Switches
        .FromSql("select DISTINCT Id,Name from table(lcr_tg_import_iapi.get_switch_lst)")
      );
    }
  }
}
