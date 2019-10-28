using Microsoft.Extensions.Logging;

namespace LCR.Import.Web.Api.Controllers
{
  public class FileUploadController : BaseApiController
  {
    public FileUploadController(
      ILogger<FileUploadController> logger
      ) : base(logger)
    {

    }
  }
}
