using Microsoft.AspNetCore.Http;

namespace LCR.Import.Web.Api.ViewModels
{
  public class UploadRequest
  {
    public int UserId { get; set; }
    public int SwitchId { get; set; }
    public IFormFile File { get; set; }
  }
}
