using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class FileUploadController : BaseApiController
  {
    public FileUploadController(
      ILogger<FileUploadController> logger,
      IConfiguration config
      ) : base(logger)
    {
      this.StorageDirectoryPath = config.GetValue<string>("StorageDirectory");
    }

    private const long _maxFileUplodSize = 1024 * 1024 * 50;

    public string StorageDirectoryPath { get; }

    /// <summary>
    /// Upload a file
    /// </summary>
    /// <param name="file">File</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesDefaultResponseType]
    [RequestSizeLimit(1024 * 1024 * 50)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadFiles(IFormFile file)
    {
      if (file == null || file.Length > _maxFileUplodSize)
      {
        return BadRequest();
      }

      try
      {
        var fileName = Path.GetFileName(file.FileName);

        var storageDayPath = Path.Combine(this.StorageDirectoryPath, DateTime.Now.ToShortDateString(), "1");

        if(!Directory.Exists(storageDayPath))
        {
          Directory.CreateDirectory(storageDayPath);
        }

        var storeFullPath = Path.Combine(storageDayPath, "current.xls");

        using (var fw = new FileStream(storeFullPath, System.IO.FileMode.Create))
        {
          file.CopyTo(fw);
          fw.Flush();
          fw.Close();
        }
      }
      catch (Exception ex)
      {
        this.Logger.LogError(ex, "Error on upload file");
        return StatusCode(500, "Unexpected error");
      }
      return Ok(new { Status = "Ok" });
    }
  }
}
