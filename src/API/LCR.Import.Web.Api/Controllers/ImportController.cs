using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class ImportController : BaseApiController
  {
    public ImportController(
      ILogger<ImportController> logger,
      IConfiguration config
      ) : base(logger)
    {
      this.StorageDirectoryPath = config.GetValue<string>("StorageDirectory");
    }

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
          while(!reader.IsClosed && reader.Name != "Направления")
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
              var test = reader.GetString(1);
            }
          } while (reader.NextResult());
        }
      }

      return Ok();
    }
  }
}
