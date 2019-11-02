using System;

namespace LCR.Import.Web.Api.Resources
{
  public interface ICommand
  {
    Guid CorrelationId { get; set; }
  }
}
