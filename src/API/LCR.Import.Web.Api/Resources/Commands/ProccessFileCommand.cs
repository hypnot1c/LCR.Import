using System;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommand : ICommand
  {
    public Guid CorrelationId { get; set; }
    public decimal UserId { get; set; }
    public decimal ImportHistoryId { get; set; }
    public string FilePath { get; set; }
  }
}
