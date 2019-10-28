using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LCR.Import.Web.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class BaseApiController : Controller
  {
    public BaseApiController(
      ILogger<BaseApiController> logger
      )
    {
      this.Logger = logger;
    }

    public ILogger<BaseApiController> Logger { get; }
  }
}
