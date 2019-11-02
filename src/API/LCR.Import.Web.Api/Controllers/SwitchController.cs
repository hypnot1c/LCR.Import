using LCR.DataService.Abstractions;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class SwitchController : BaseApiController
  {
    public SwitchController(
      IDataService dataService,
      ILogger<SwitchController> logger
      ) : base(logger)
    {
      this.DataService = dataService;
    }

    public IDataService DataService { get; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SwitchModel>>> GetList()
    {
      var list = await this.DataService.Switch_GetListAsync();
      return Ok(list);
    }
  }
}
