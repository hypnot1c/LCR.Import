using AutoMapper;
using LCR.DataService.Abstractions;
using LCR.Import.Web.Api.Resources;
using LCR.Import.Web.Api.ViewModels;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class ImportController : BaseApiController
  {
    public ImportController(
      IDataService dataService,
      TPMContext tpmCtx,
      IMapper mapper,
      ILogger<ImportController> logger
      ) : base(logger)
    {
      this.DataService = dataService;
      this.TPMContext = tpmCtx;
      this.Mapper = mapper;
    }

    public IDataService DataService { get; }
    public TPMContext TPMContext { get; }
    public IMapper Mapper { get; }

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
      var data = this.TPMContext.ImportResults
        .Where(d => d.UploadHistoryId == id)
        .OrderBy(rd => rd.DataRowId)
        .AsQueryable()
        ;

      var tempres = await data.ToListAsync();

      var result = await PaginatedList<ImportResultQueryModel>.CreateAsync(data.AsNoTracking(), page, pageSize);

      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count } });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetUploadHistory(int page = 1, int pageSize = 10)
    {
      var data = this.TPMContext.UploadHistory
        .OrderByDescending(h => h.DateUpload)
        .AsQueryable();

      var result = await PaginatedList<UploadHistoryModel>.CreateAsync(data.AsNoTracking(), page, pageSize);
      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count }});
    }

    [HttpGet("{id:int}/summary")]
    public async Task<IActionResult> GetImportSummary(int id)
    {
      var data = await this.Mapper.ProjectTo<ImportSummaryViewModel>(this.TPMContext.UploadHistory
        .Where(h => h.Id == id)
        )
        .SingleOrDefaultAsync()
        ;

      var switches = await this.DataService.Switch_GetListAsync();

      data.SwitchName = switches.SingleOrDefault(s => s.Id == data.SwitchId)?.Name;

      return Ok(new { Status = "Ok", result = data });
    }

    [HttpGet("{id}/is-all-approved")]
    public async Task<IActionResult> IsAllApproved(decimal id)
    {
      var isAllApproved = !(await this.TPMContext.ImportMappedData
        .AnyAsync(r => (r.Approved == null || !r.Approved.Value) && !r.Excluded)
        )
        ;

      return Ok(new { Status = "Ok", result = isAllApproved });
    }

    [HttpPost("{id}/row/{rowId}/approved")]
    public async Task<IActionResult> SetApproved(decimal id, decimal rowId, [FromBody] SetApprovedRowRequest vm)
    {
      var mappedData = await this.TPMContext.ImportMappedData
        .Where(md => md.ImportRawDataId == rowId)
        .SingleOrDefaultAsync()
        ;

      mappedData.Approved = vm.Approved;

      await this.TPMContext.SaveChangesAsync();

      return Ok(new { Status = "Ok" });
    }

    [HttpPost("{id}/row/{rowId}/excluded")]
    public async Task<IActionResult> SetExcluded(decimal id, decimal rowId, [FromBody] SetExcludedRowRequest vm)
    {
      var mappedData = await this.TPMContext.ImportMappedData
        .Where(md => md.ImportRawDataId == rowId)
        .SingleOrDefaultAsync()
        ;

      mappedData.Excluded = vm.Excluded;

      await this.TPMContext.SaveChangesAsync();

      return Ok(new { Status = "Ok" });
    }
  }
}
