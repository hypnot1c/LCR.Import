using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.ViewModels
{
  public class SaveRowRequest
  {
    public decimal LCROperatorId { get; set; }
    public DateTime? LCRDateClose { get; set; }
  }
}
