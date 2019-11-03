using LCR.TPM.Model.Abstractions;

namespace LCR.TPM.Model
{
  public class ImportRawDataModel : BaseModel
  {
    public UploadHistoryModel UploadHistory { get; set; }
    public int UploadHistoryId { get; set; }
    public string DataRowId { get; set; }
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
    public ImportMappedDataModel ImportMappedData { get; set; }
  }
}
