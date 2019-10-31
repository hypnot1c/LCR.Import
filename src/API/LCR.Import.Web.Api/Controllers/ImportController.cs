using ExcelDataReader;
using LCR.Import.Web.Api.Resources;
using LCR.TPM.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class ImportController : BaseApiController
  {
    public ImportController(
      TPMContext tpmCtx,
      ILogger<ImportController> logger,
      IConfiguration config
      ) : base(logger)
    {
      this.TPMContext = tpmCtx;
      this.StorageDirectoryPath = config.GetValue<string>("StorageDirectory");
    }

    public TPMContext TPMContext { get; }
    public string StorageDirectoryPath { get; }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> ProccessFile(int userId = 1)
    {
      var storageDayPath = Path.Combine(this.StorageDirectoryPath, DateTime.Now.ToShortDateString(), "1");

      var storeFullPath = Path.Combine(storageDayPath, "current.xls");

      using (var stream = System.IO.File.Open(storeFullPath, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          while (!reader.IsClosed && reader.Name != "Направления")
          {
            reader.NextResult();
          }
          do
          {
            while (reader.Read())
            {
              var firstValue = reader.GetValue(0);
              if (firstValue == null || !Int32.TryParse(firstValue.ToString(), out var number))
              {
                continue;
              }

              var rawData = reader.ToImportRawDataModel();

              this.TPMContext.ImportRawData.Add(rawData);
            }
          } while (reader.NextResult());
        }
      }
      await this.TPMContext.SaveChangesAsync();

      var data = await this.TPMContext.ImportRawData.ToListAsync();

      return Ok(data);
    }
  }
}
