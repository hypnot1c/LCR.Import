using LCR.TPM.Model.Abstractions;
using System;
using System.Collections.Generic;

namespace LCR.TPM.Model
{
  public class UploadHistoryModel : BaseModel
  {
    public UploadHistoryModel()
    {
      this.ImportRawData = new HashSet<ImportRawDataModel>();
      this.ImportMappedData = new HashSet<ImportMappedDataModel>();
    }
    public decimal UserId { get; set; }
    public decimal SwitchId { get; set; }
    public DateTime DateUpload { get; set; }
    public string FileName { get; set; }
    public int RowsAffected { get; set; }
    public ImportStep Step { get; set; }
    public ICollection<ImportRawDataModel> ImportRawData { get; set; }
    public ICollection<ImportMappedDataModel> ImportMappedData { get; set; }
  }
}
