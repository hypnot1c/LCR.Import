using System;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommand : ICommand
  {
    public Guid CorrelationId { get; set; }
    public int UserId { get; set; }
    public int ImportHistoryId { get; set; }
    public string FilePath { get; set; }
  }
}
