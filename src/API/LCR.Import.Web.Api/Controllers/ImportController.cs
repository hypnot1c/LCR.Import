using AutoMapper;
using LCR.DataService.Abstractions;
using LCR.Import.Web.Api.Resources;
using LCR.Import.Web.Api.ViewModels;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
    public async Task<IActionResult> GetResult(decimal id, int userId = 1, int page = 1, int pageSize = 50, int? rowFilter = null)
    {
      var data = this.TPMContext.ImportResults
        .Where(d => d.UploadHistoryId == id)
        //.Where(d => d.FormatFlags == null)
        .OrderBy(rd => rd.DataRowId)
        .AsQueryable()
        ;

      switch(rowFilter)
      {
        case 1:
          data = data.Where(i => i.FormatFlags == null && (i.LogicFlags == 2 || (i.LogicFlags != 0 && i.LogicFlags != 16)) && i.Approved != true);
          break;
        case 2:
          data = data.Where(i => i.FormatFlags != null);
          break;
        case 3:
          data = data.Where(i => i.Excluded == true);
          break;
        default:
          break;
      }

      var tempres = await data.ToListAsync();

      var result = await PaginatedList<ImportResultQueryModel>.CreateAsync(data.AsNoTracking(), page, pageSize);

      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count } });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetUploadHistory(int page = 1, int pageSize = 10, DateTime? dateFrom = null, DateTime? dateTo = null, int? switchId = null)
    {
      var data = this.Mapper.ProjectTo<ImportHistoryViewModel>(this.TPMContext.UploadHistory
        .Where(h => h.Step == ImportStep.Saved)
        .OrderByDescending(h => h.DateUpload)
        )
        .AsQueryable()
        ;

      if(dateFrom != null)
      {
        data = data.Where(d => d.DateUpload >= dateFrom);
      }
      if (dateTo != null)
      {
        data = data.Where(d => d.DateUpload >= dateTo);
      }
      if (switchId != null)
      {
        data = data.Where(d => d.SwitchId == switchId);
      }


      var switches = await this.DataService.Switch_GetListAsync();

      var result = await PaginatedList<ImportHistoryViewModel>.CreateAsync(data.AsNoTracking(), page, pageSize);
      result.ForEach(i =>
      {
        i.SwitchName = switches.SingleOrDefault(s => s.Id == i.SwitchId)?.Name;
      });

      return Ok(new { Status = "Ok", result = new { data = result, result.TotalPages, result.Count } });
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
        .Where(md => md.UploadHistoryId == id)
        .Where(r => !r.Excluded)
        .Where(r => r.Flags != 2)
        .AnyAsync(r => (r.Approved == null || !r.Approved.Value))
        )
        ;

      return Ok(new { Status = "Ok", result = isAllApproved });
    }

    [HttpPost("{id}/row/{rowId}/approved")]
    public async Task<IActionResult> SetApproved(decimal id, decimal rowId, [FromBody] SetApprovedRowRequest vm)
    {
      var mappedData = await this.TPMContext.ImportMappedData
        .Where(md => md.UploadHistoryId == id)
        .Where(md => md.ImportRawDataId == rowId)
        .SingleOrDefaultAsync()
        ;

      mappedData.Approved = vm.Approved;
      mappedData.Excluded = false;
      mappedData.LCRDateClose = mappedData.LCRDateClose ?? mappedData.FileDateClose;

      if (mappedData.LCRTGId == null)
      {
        mappedData.LCRDateOpen = mappedData.FileDateOpen;
        mappedData.LCRDirection = mappedData.FileDirection;
        mappedData.LCROperatorId = mappedData.LCROperatorId ?? mappedData.FileOperatorId;
      }

      await this.TPMContext.SaveChangesAsync();

      var data = await this.TPMContext.ImportResults
       .AsNoTracking()
       .Where(d => d.UploadHistoryId == id)
       .Where(d => d.FormatFlags == null)
       .Where(d => d.Id == rowId)
       .SingleAsync()
       ;

      return Ok(new { Status = "Ok", result = data });
    }

    [HttpPost("{id}/row/{rowId}/excluded")]
    public async Task<IActionResult> SetExcluded(decimal id, decimal rowId, [FromBody] SetExcludedRowRequest vm)
    {
      var mappedData = await this.TPMContext.ImportMappedData
        .Where(md => md.UploadHistoryId == id)
        .Where(md => md.ImportRawDataId == rowId)
        .SingleOrDefaultAsync()
        ;

      mappedData.Excluded = vm.Excluded;
      mappedData.Approved = false;

      await this.TPMContext.SaveChangesAsync();

      return Ok(new { Status = "Ok" });
    }

    [HttpPost("{id}/lcr-save")]
    public async Task<IActionResult> LCRSave(decimal id)
    {
      var isAllApproved = !(await this.TPMContext.ImportMappedData
        .Where(md => md.UploadHistoryId == id)
        .AnyAsync(r => (r.Approved == null || !r.Approved.Value) && !r.Excluded)
        )
        ;

      if (!isAllApproved)
      {
        this.Logger.LogError("Not all rows are approved for saving");
        return StatusCode(400, new { Status = "Error" });
      }

      try
      {
        await this.DataService.Import_SaveAsync(id);
        return Ok(new { Status = "Ok" });
      }
      catch (Exception ex)
      {
        this.Logger.LogError(ex, "Error saving data to lcr");
        return StatusCode(500, new { Status = "Error" });
      }
    }

    [HttpPost("{id}/row/{rowId}/save")]
    public async Task<IActionResult> SaveRow(decimal id, decimal rowId, [FromBody] SaveRowRequest vm)
    {
      var mappedData = await this.TPMContext.ImportMappedData
        .Where(md => md.UploadHistoryId == id)
        .Where(md => md.ImportRawDataId == rowId)
        .SingleOrDefaultAsync()
        ;

      mappedData.LCROperatorId = vm.LCROperatorId;
      mappedData.LCRDateClose = vm.LCRDateClose;
      mappedData.Flags = 0;

      await this.TPMContext.SaveChangesAsync();

      await this.DataService.MappedData_SetRowFlagsAsync(id, rowId);

      var data = await this.TPMContext.ImportResults
        .AsNoTracking()
        .Where(d => d.UploadHistoryId == id)
        .Where(d => d.FormatFlags == null)
        .Where(d => d.Id == rowId)
        .SingleAsync()
        ;

      return Ok(new { Status = "Ok", result = data });
    }
  }
}
