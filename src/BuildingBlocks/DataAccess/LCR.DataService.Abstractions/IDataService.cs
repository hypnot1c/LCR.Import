using LCR.TPM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LCR.DataService.Abstractions
{
  public interface IDataService
  {
    Task<IEnumerable<SwitchModel>> Switch_GetListAsync();
  }
}
