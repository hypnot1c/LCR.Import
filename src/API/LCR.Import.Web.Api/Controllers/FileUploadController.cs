using LCR.Import.Web.Api.Resources;
using LCR.TPM.Context;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class FileUploadController : BaseApiController
  {
    public FileUploadController(
      TPMContext tpmCtx,
      IBackgroundQueue<ICommand> fileCommandQueue,
      ILogger<FileUploadController> logger,
      IConfiguration config
      ) : base(logger)
    {
      this.TPMContext = tpmCtx;
      this.FileCommandQueue = fileCommandQueue;
      this.StorageDirectoryPath = config.GetValue<string>("StorageDirectory");
    }

    private const long _maxFileUplodSize = 1024 * 1024 * 50;

    public TPMContext TPMContext { get; }
    public IBackgroundQueue<ICommand> FileCommandQueue { get; }
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
    public async Task<ActionResult> UploadFiles([FromForm] UploadRequest vm)
    {
      var file = vm.File;
      if (file == null || file.Length > _maxFileUplodSize)
      {
        return BadRequest();
      }

      try
      {
        var userId = vm.UserId;

        var fileName = Path.GetFileName(file.FileName);

        var storageDayPath = Path.Combine(this.StorageDirectoryPath, DateTime.Now.ToShortDateString(), userId.ToString());

        if (!Directory.Exists(storageDayPath))
        {
          Directory.CreateDirectory(storageDayPath);
        }

        var storeFullPath = Path.Combine(storageDayPath, "current.xls");

        using (var fw = new FileStream(storeFullPath, FileMode.Create))
        {
          file.CopyTo(fw);
          fw.Flush();
          fw.Close();
        }

        var history = new UploadHistoryModel();
        history.UserId = userId;
        history.FileName = fileName;
        history.SwitchId = 0;
        history.DateUpload = DateTime.Now.Date;
        history.Step = ImportStep.Pending;

        this.TPMContext.UploadHistory.Add(history);
        await this.TPMContext.SaveChangesAsync();

        var command = new ProccessFileCommand { UserId = userId, ImportHistoryId = history.Id, FilePath = storeFullPath };
        this.FileCommandQueue.Enqueue(command);
        return Ok(new { Status = "Ok", HistoryId = history.Id });
      }
      catch (Exception ex)
      {
        this.Logger.LogError(ex, "Error on upload file");
        return StatusCode(500, "Unexpected error");
      }
    }
  }

  public class UploadRequest
  {
    public int UserId { get; set; }
    public IFormFile File { get; set; }
  }
}
