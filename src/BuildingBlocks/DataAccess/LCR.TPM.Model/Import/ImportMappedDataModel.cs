using LCR.TPM.Model.Abstractions;
using System;

namespace LCR.TPM.Model
{
  public class ImportMappedDataModel : BaseModel
  {
    public UploadHistoryModel UploadHistory { get; set; }
    public decimal UploadHistoryId { get; set; }
    public ImportRawDataModel ImportRawData { get; set; }
    public decimal ImportRawDataId { get; set; }
    public decimal? FileOperatorId { get; set; }
    public char FileDirection { get; set; }
    public DateTime? FileDateOpen { get; set; }
    public DateTime? FileDateClose { get; set; }
    public decimal? LCRTGId { get; set; }
    public decimal? LCROperatorId { get; set; }
    public char? LCRDirection { get; set; }
    public DateTime? LCRDateOpen { get; set; }
    public DateTime? LCRDateClose { get; set; }
    public long Flags { get; set; }
    public bool? Approved { get; set; }
    public bool Excluded { get; set; }
  }
}
