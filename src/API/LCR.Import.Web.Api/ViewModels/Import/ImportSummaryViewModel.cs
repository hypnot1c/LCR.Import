using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.ViewModels
{
  public class ImportSummaryViewModel
  {
    public decimal SwitchId { get; set; }
    public string SwitchName { get; set; }
    public string FileName { get; set; }
    public int RowsCount { get; set; }
    public int DiffRowsCount { get; set; }
    public int ErrorRowsCount { get; set; }
  }
}
