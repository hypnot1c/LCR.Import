using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommandHostedService : BackgroundService
  {
    public ProccessFileCommandHostedService(
      IServiceScopeFactory scopeFactory,
      IBackgroundQueue<ICommand> taskQueue,
      ILogger<ProccessFileCommandHostedService> logger
      )
    {
      this.TaskQueue = taskQueue;
      this._scopeFactory = scopeFactory;
      this._logger = logger;
    }

    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public IBackgroundQueue<ICommand> TaskQueue { get; }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
      this._logger.LogInformation("File commands hosted service is starting.");

      while (!cancellationToken.IsCancellationRequested)
      {
        var workItem = await TaskQueue.DequeueAsync(cancellationToken);

        try
        {
          using (var scope = this._scopeFactory.CreateScope())
          {
            var concreteType = typeof(IProccessFileCommandService<>).MakeGenericType(workItem.GetType());
            var commandService = scope.ServiceProvider.GetRequiredService(concreteType);
            await (Task)concreteType.GetMethod("OnNextAsync").Invoke(commandService, new object[] { workItem });
          }
        }
        catch (Exception ex)
        {
          this._logger.LogError(ex, "Error occurred executing {0}.", nameof(workItem));
        }
      }

      this._logger.LogInformation("File commands hosted service is stopping.");
    }
  }
}
