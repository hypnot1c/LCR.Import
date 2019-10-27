using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Helpers.AspNet
{
  public class GlobalExceptionFilter : IAsyncExceptionFilter, IExceptionFilter
  {
    private readonly ILogger<GlobalExceptionFilter> logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
      this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      this.logger.LogError(context.Exception, "Unhandled exception");
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
      this.logger.LogError(context.Exception, "Unhandled exception");
      return Task.CompletedTask;
    }
  }
}
