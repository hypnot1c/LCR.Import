using LCR.TPM.Model.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCR.TPM.Model
{
  public class ImportResultQueryModel : BaseModel
  {
    public decimal UploadHistoryId { get; set; }
    public decimal? DataRowId { get; set; }
    public string ChannelBundleName { get; set; }
    public string SwitchOperatorName { get; set; }
    public string PairedSwitchOperatorFullName { get; set; }
    public string Direction { get; set; }
    public string DirectionType { get; set; }
    public string OperatorsNetworkConnectionLvl { get; set; }
    public string RTNetworkConnectionLevel { get; set; }
    public decimal? FileOperatorId { get; set; }
    public char? FileDirection { get; set; }
    public DateTime? FileDateOpen { get; set; }
    public DateTime? FileDateClose { get; set; }
    public decimal? LCROperatorId { get; set; }
    public char? LCRDirection { get; set; }
    public DateTime? LCRDateOpen { get; set; }
    public DateTime? LCRDateClose { get; set; }
    public long? FormatFlags { get; set; }
    public long? LogicFlags { get; set; }
    public string FileOperatorName { get; set; }
    public decimal? LCRTGID { get; set; }
    public string LCROperatorName { get; set; }
    public bool? Approved { get; set; }
    public bool? Excluded { get; set; }
    public string ValidFrom { get; set; }
    public string ValidUntil { get; set; }
  }
}
