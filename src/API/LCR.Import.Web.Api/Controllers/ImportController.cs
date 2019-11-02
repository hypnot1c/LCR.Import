using LCR.TPM.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class ImportController : BaseApiController
  {
    public ImportController(
      TPMContext tpmCtx,
      IConfiguration config,
      ILogger<ImportController> logger
      ) : base(logger)
    {
      this.TPMContext = tpmCtx;
    }

    public TPMContext TPMContext { get; }

    [HttpGet("{id}/user/{userId:int}/status")]
    public async Task<IActionResult> GetFileStatus(int id, int userId = 1)
    {
      var historyEntry = await this.TPMContext.ImportHistory
        .Where(ih => ih.UserId == userId)
        .Where(ih => ih.Id == id)
        .SingleAsync()
        ;

      return Ok(new { Status = "Ok", HistoryStatus = historyEntry.Step });
    }
  }
}
