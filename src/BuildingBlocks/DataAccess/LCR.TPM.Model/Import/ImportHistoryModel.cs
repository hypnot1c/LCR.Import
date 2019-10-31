using LCR.TPM.Model.Abstractions;
using System;

namespace LCR.TPM.Model
{
  public class ImportHistoryModel : BaseModel
  {
    public int UserId { get; set; }
    public int SwitchId { get; set; }
    public DateTime DateUpload { get; set; }
    public string FileName { get; set; }
    public int RowsAffected { get; set; }
    public ImportStep Step { get; set; }
  }
}
