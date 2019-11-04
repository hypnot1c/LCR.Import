using LCR.TPM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LCR.TPM.Context.Configurations
{
  public class ImportRawDataConfiguration : IEntityTypeConfiguration<ImportRawDataModel>
  {
    public void Configure(EntityTypeBuilder<ImportRawDataModel> builder)
    {
      builder.ToTable("UPLOAD_RAWDATA");
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Id).HasColumnName("ID").HasDefaultValue();
      builder.Property(p => p.UploadHistoryId).HasColumnName("UPLOADHISTORYID");
      builder.Property(p => p.DataRowId).HasColumnName("DARAROWID");
      builder.Property(p => p.ChannelBundleName).HasColumnName("TRUNKGROUPNAME");
      builder.Property(p => p.SwitchOperatorName).HasColumnName("SWITCHOPERATORNAME");
      builder.Property(p => p.ATES).HasColumnName("SWITCH");
      builder.Property(p => p.ChannelBundleOperatorName).HasColumnName("TRUNKGROUPOPERATORNAME");
      builder.Property(p => p.Direction).HasColumnName("DIRECTION");
      builder.Property(p => p.DirectionType).HasColumnName("TRUNKGROUPTYPE");
      builder.Property(p => p.AlarmType).HasColumnName("SIGNALINGTYPE");
      builder.Property(p => p.DateOpen).HasColumnName("VALIDFROM");
      builder.Property(p => p.DateClose).HasColumnName("VALIDUNTIL");
      builder.Property(p => p.PairedSwitchOperatorFullName).HasColumnName("PAIREDSWITCHOPERATORFULLNAME");
      builder.Property(p => p.PairedSwitchOperatorCoverage).HasColumnName("PAIREDSWITCHOPERATORCOVERAGE");
      builder.Property(p => p.OperatorsNetworkConnectionLevel).HasColumnName("OPERATORSNETWORKCONNECTLVL");
      builder.Property(p => p.RTNetworkConnectionLevel).HasColumnName("RTNETWORKCONNECTIONLEVEL");
      builder.Property(p => p.BranchOffice).HasColumnName("BRANCHOFFICE");
      builder.Property(p => p.ShortName).HasColumnName("SHORTNAME");

      builder.HasOne(p => p.UploadHistory).WithMany(p => p.ImportRawData).HasForeignKey(p => p.UploadHistoryId);

    }
  }
}
