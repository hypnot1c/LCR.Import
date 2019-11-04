using LCR.Import.Web.Api.Resources;
using LCR.Import.Web.Api.ViewModels;
using LCR.TPM.Context;
using LCR.TPM.Model;
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
      var historyEntry = await this.TPMContext.UploadHistory
        .Where(ih => ih.UserId == userId)
        .Where(ih => ih.Id == id)
        .SingleAsync()
        ;

      return Ok(new { Status = "Ok", HistoryStatus = historyEntry.Step });
    }

    [HttpGet("{id}/user/{userId:int}/result")]
    public async Task<IActionResult> GetResult(decimal id, int userId = 1, int page = 1, int pageSize = 10)
    {
      var data = this.TPMContext.ImportRawData
        .Where(d => d.UploadHistoryId == id)
        .Where(d => d.ImportMappedData != null)
        //.Where(ih => ih.UploadHistory.UserId == userId)
        .Select(d => new ImportResultViewModel
        {
          DataRowId = d.DataRowId,
          ChannelBundleName = d.ChannelBundleName,
          SwitchOperatorName = d.SwitchOperatorName,
          PairedSwitchOperatorFullName = d.PairedSwitchOperatorFullName,
          DirectionType = d.DirectionType,
          Direction = d.Direction,
          OperatorsNetworkConnectionLvl = d.OperatorsNetworkConnectionLevel,
          RTNetworkConnectionLevel = d.RTNetworkConnectionLevel,
          DateOpen = d.DateOpen,
          DateClose = d.DateClose,
          Fdate = d.ImportMappedData.FileDateOpen
        })
        .AsQueryable()
        ;

      var result = await PaginatedList<ImportResultViewModel>.CreateAsync(data.AsNoTracking(), page, pageSize);

      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count } });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetUploadHistory(int page = 1, int pageSize = 10)
    {
      var data = this.TPMContext.UploadHistory.AsQueryable();

      var result = await PaginatedList<UploadHistoryModel>.CreateAsync(data.AsNoTracking(), page, pageSize);
      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count }});
    }
  }
}
