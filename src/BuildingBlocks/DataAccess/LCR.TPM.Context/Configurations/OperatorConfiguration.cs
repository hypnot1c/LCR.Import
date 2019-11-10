using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCR.TPM.Context.Configurations
{
  public class OperatorConfiguration : IQueryTypeConfiguration<OperatorModel>
  {
    public void Configure(QueryTypeBuilder<OperatorModel> builder)
    {
      builder.Property(p => p.Id).HasColumnName("ID");
      builder.Property(p => p.Name).HasColumnName("NAME");
    }
  }
}
