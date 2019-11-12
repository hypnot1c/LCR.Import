using LCR.DataService.Abstractions;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class OperatorsController : BaseApiController
  {
    public OperatorsController(
      IDataService dataService,
      ILogger<OperatorsController> logger
      ) : base(logger)
    {
      this.DataService = dataService;
    }

    public IDataService DataService { get; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorModel>>> GetList()
    {
      var list = (await this.DataService.Operator_GetListAsync())
        .OrderBy(s => s.Name)
        ;

      return Ok(new { Status = "Ok", result = list });
    }
  }
}
