using System;

namespace LCR.Import.Web.Api.ViewModels
{
  public class ImportResultViewModel
  {
    public decimal? DataRowId { get; set; }
    public string ChannelBundleName { get; set; }
    public string SwitchOperatorName { get; set; }
    public string PairedSwitchOperatorFullName { get; set; }
    public string DirectionType { get; set; }
    public string OperatorsNetworkConnectionLvl { get; set; }
    public string RTNetworkConnectionLevel { get; set; }
    public char? FileDirection { get; set; }
    public DateTime? FileDateOpen { get; set; }
    public DateTime? FileDateClose { get; set; }
    public string LCRSwitchOperatorId { get; set; }
    public char? LCRDirection { get; set; }
    public DateTime? LCRDateOpen { get; set; }
    public DateTime? LCRDateClose { get; set; }
  }
}
