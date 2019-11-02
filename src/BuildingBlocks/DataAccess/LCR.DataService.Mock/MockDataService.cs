using LCR.DataService.Abstractions;
using LCR.TPM.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.DataService.Mock
{
  public class MockDataService : IDataService
  {
    public Task<IEnumerable<SwitchModel>> Switch_GetListAsync()
    {
      var result = new SwitchModel[]
      {
        new SwitchModel { Id = 1, Name = "Коммутатор 1" },
        new SwitchModel { Id = 2, Name = "Коммутатор 2" },
        new SwitchModel { Id = 3, Name = "Коммутатор 3" },
        new SwitchModel { Id = 4, Name = "Коммутатор 4" },
        new SwitchModel { Id = 4, Name = "Коммутатор 5" }
      };

      return Task.FromResult(result.AsEnumerable());
    }
  }
}
