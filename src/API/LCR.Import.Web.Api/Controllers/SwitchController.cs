using LCR.DataService.Abstractions;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class SwitchController : BaseApiController
  {
    public SwitchController(
      IDataService dataService,
      TPMContext tpmCtx,
      ILogger<SwitchController> logger
      ) : base(logger)
    {
      this.DataService = dataService;
      this.TPMContext = tpmCtx;
    }

    public IDataService DataService { get; }
    public TPMContext TPMContext { get; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SwitchModel>>> GetList()
    {
      //var list = Enumerable.Repeat(new SwitchModel(), 5);
      var list = (await this.TPMContext.Switches
        .FromSql("select Id,Name from table(lcr_tg_import_iapi.get_switch_lst)")
        .ToListAsync())
        .OrderBy(s => s.Name)
        ;

      return Ok(list);
    }
  }
}
