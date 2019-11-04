using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.ViewModels
{
  public class ImportResultViewModel
  {
    public decimal? DataRowId { get; set; }
    public string ChannelBundleName { get; set; }
    public string SwitchOperatorName { get; set; }
    public string PairedSwitchOperatorFullName { get; set; }
    public string DirectionType { get; set; }
    public string Direction { get; set; }
    public string OperatorsNetworkConnectionLvl { get; set; }
    public string RTNetworkConnectionLevel { get; set; }
    public string DateOpen { get; set; }
    public string DateClose { get; set; }
    public DateTime? Fdate { get; set; }
  }
}
