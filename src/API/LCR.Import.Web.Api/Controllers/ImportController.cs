using AutoMapper;
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
      TPMContext tpmCtx,
      IMapper mapper,
      ILogger<ImportController> logger
      ) : base(logger)
    {
      this.TPMContext = tpmCtx;
      this.Mapper = mapper;
    }

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
      var data = this.Mapper.ProjectTo<ImportResultViewModel>(
        this.TPMContext.ImportRawData
          .Where(d => d.UploadHistoryId == id)
        )
        .OrderBy(rd => rd.DataRowId)
        .AsQueryable()
        ;

      var result = await PaginatedList<ImportResultViewModel>.CreateAsync(data.AsNoTracking(), page, pageSize);

      result.ForEach(e =>
      {
        if(e.FileDateOpen != e.LCRDateOpen)
        {
          e.Flags |= 4;
        }
        if (e.FileDateClose != e.LCRDateClose)
        {
          e.Flags |= 8;
        }

        if (e.FileDirection != e.LCRDirection)
        {
          e.Flags |= 16;
        }
      });

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
  }
}
