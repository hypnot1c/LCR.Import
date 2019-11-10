using LCR.TPM.Model.Abstractions;
using System;

namespace LCR.TPM.Model
{
  public class ImportRawDataModel : BaseModel
  {
    public UploadHistoryModel UploadHistory { get; set; }
    public decimal UploadHistoryId { get; set; }
    public decimal? DataRowId { get; set; }
    public string ChannelBundleName { get; set; }
    public string SwitchOperatorName { get; set; }
    public string ATES { get; set; }
    public string ChannelBundleOperatorName { get; set; }
    public string Direction { get; set; }
    public string DirectionType { get; set; }
    public string AlarmType { get; set; }
    public string DateOpen { get; set; }
    public string DateClose { get; set; }
    public string PairedSwitchOperatorFullName { get; set; }
    public string PairedSwitchOperatorCoverage { get; set; }
    public string OperatorsNetworkConnectionLevel { get; set; }
    public string RTNetworkConnectionLevel { get; set; }
    public string BranchOffice { get; set; }
    public string ShortName { get; set; }
    public ImportMappedDataModel ImportMappedData { get; set; }
    public ImportFormatErrorsModel ImportFormatErrors { get; set; }

    public bool IsEmpty
    {
      get
      {
        return String.IsNullOrEmpty(this.ChannelBundleName)
          && String.IsNullOrEmpty(this.SwitchOperatorName)
          && String.IsNullOrEmpty(this.ATES)
          && String.IsNullOrEmpty(this.ChannelBundleOperatorName)
          && String.IsNullOrEmpty(this.Direction)
          && String.IsNullOrEmpty(this.DirectionType)
          && String.IsNullOrEmpty(this.AlarmType)
          && String.IsNullOrEmpty(this.DateOpen)
          && String.IsNullOrEmpty(this.DateClose)
          && String.IsNullOrEmpty(this.PairedSwitchOperatorFullName)
          && String.IsNullOrEmpty(this.PairedSwitchOperatorCoverage)
          && String.IsNullOrEmpty(this.OperatorsNetworkConnectionLevel)
          && String.IsNullOrEmpty(this.RTNetworkConnectionLevel)
          && String.IsNullOrEmpty(this.BranchOffice)
          && String.IsNullOrEmpty(this.ShortName);
      }
    }
  }
}
