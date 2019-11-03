using LCR.TPM.Model.Abstractions;
using System;

namespace LCR.TPM.Model
{
  public class ImportMappedDataModel : BaseModel
  {
    public UploadHistoryModel UploadHistory { get; set; }
    public int UploadHistoryId { get; set; }
    public ImportRawDataModel ImportRawData { get; set; }
    public int ImportRawDataId { get; set; }
    public int FileOperatorId { get; set; }
    public char FileDirection { get; set; }
    public DateTime FileDateOpen { get; set; }
    public DateTime? FileDateClose { get; set; }
    public int LCROperatorId { get; set; }
    public char LCRDirection { get; set; }
    public DateTime? LCRDateOpen { get; set; }
    public DateTime? LCRDateClose { get; set; }
  }
}
