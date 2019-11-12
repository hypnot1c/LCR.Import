using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCR.TPM.Context.Configurations
{
  public class ImportResultConfiguration : IQueryTypeConfiguration<ImportResultQueryModel>
  {
    public void Configure(QueryTypeBuilder<ImportResultQueryModel> builder)
    {
      builder.Property(p => p.Id).HasColumnName("ID");
      builder.Property(p => p.DataRowId).HasColumnName("DARAROWID");
      builder.Property(p => p.UploadHistoryId).HasColumnName("UPLOADHISTORYID");
      builder.Property(p => p.ChannelBundleName).HasColumnName("TRUNKGROUPNAME");
      builder.Property(p => p.DirectionType).HasColumnName("TRUNKGROUPTYPE");
      builder.Property(p => p.Direction).HasColumnName("DIRECTION");
      builder.Property(p => p.OperatorsNetworkConnectionLvl).HasColumnName("OPERATORSNETWORKCONNECTLVL");
      builder.Property(p => p.PairedSwitchOperatorFullName).HasColumnName("PAIREDSWITCHOPERATORFULLNAME");
      builder.Property(p => p.RTNetworkConnectionLevel).HasColumnName("RTNETWORKCONNECTIONLEVEL");
      builder.Property(p => p.SwitchOperatorName).HasColumnName("SWITCHOPERATORNAME");
      builder.Property(p => p.ValidFrom).HasColumnName("VALIDFROM");
      builder.Property(p => p.ValidUntil).HasColumnName("VALIDUNTIL");
      builder.Property(p => p.FormatFlags).HasColumnName("FORMATFLAGS");
      builder.Property(p => p.LogicFlags).HasColumnName("LOGICFLAGS");
      builder.Property(p => p.FileOperatorId).HasColumnName("FILE_TGOPERATORID");
      builder.Property(p => p.FileDirection).HasColumnName("FILE_DIRECTION");
      builder.Property(p => p.FileDateOpen).HasColumnName("FILE_VALIDFROM");
      builder.Property(p => p.FileDateClose).HasColumnName("FILE_VALIDUNTIL");
      builder.Property(p => p.FileOperatorName).HasColumnName("FILE_OPERATORNAME");
      builder.Property(p => p.LCRTGID).HasColumnName("LCR_TGID");
      builder.Property(p => p.LCROperatorId).HasColumnName("LCR_TGOPERATORID");
      builder.Property(p => p.LCRDirection).HasColumnName("LCR_DIRECTION");
      builder.Property(p => p.LCRDateOpen).HasColumnName("LCR_VALIDFROM");
      builder.Property(p => p.LCRDateClose).HasColumnName("LCR_VALIDUNTIL");
      builder.Property(p => p.LCROperatorName).HasColumnName("LCR_OPERATORNAME");
      builder.Property(p => p.Approved).HasColumnName("APPROVED");
      builder.Property(p => p.Excluded).HasColumnName("EXCLUDED");
    }
  }
}
