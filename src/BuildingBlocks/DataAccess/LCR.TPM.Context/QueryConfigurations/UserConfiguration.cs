using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.QueryConfigurations
{
  public class UserConfiguration : IQueryTypeConfiguration<UserQueryModel>
  {
    public UserConfiguration(TPMContext ctx)
    {
      this.DbContext = ctx;
    }

    public TPMContext DbContext { get; }

    public void Configure(QueryTypeBuilder<UserQueryModel> builder)
    {
      builder.ToQuery(() => this.DbContext.Users
      .FromSql("select DISTINCT Id, first_name as Firstname, last_name as Lastname from table(lcr_protocol_iapi.get_user_lst(0))")
      );
    }
  }
}
