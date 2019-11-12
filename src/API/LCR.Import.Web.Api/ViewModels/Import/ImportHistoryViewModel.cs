using LCR.TPM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.ViewModels
{
  public class ImportHistoryViewModel
  {
    public decimal Id { get; set; }
    public decimal UserId { get; set; }
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
