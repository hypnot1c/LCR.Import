using LCR.TPM.Model.Abstractions;
using System;

namespace LCR.TPM.Model
{
  public class UploadHistoryQueryModel : BaseModel
  {
    public decimal UserId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public decimal SwitchId { get; set; }
    public string SwitchName { get; set; }
    private DateTime _dateUpload;
    public DateTime DateUpload
    {
      get
      {
        return DateTime.SpecifyKind(this._dateUpload, DateTimeKind.Utc);
      }
      set
      {
        this._dateUpload = value;
      }
    }
    public string FileName { get; set; }
    public int RowsAffected { get; set; }
    public ImportStep Step { get; set; }
  }
}
