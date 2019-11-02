using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public class ProccessFileCommandBackgroundQueue : IBackgroundQueue<ICommand>
  {
    private ConcurrentQueue<ICommand> _workItems =
        new ConcurrentQueue<ICommand>();
    private SemaphoreSlim _signal = new SemaphoreSlim(0);

    public void Enqueue(ICommand workItem)
    {
      if (workItem == null)
      {
        throw new ArgumentNullException(nameof(workItem));
      }

      _workItems.Enqueue(workItem);
      _signal.Release();
    }

    public async Task<ICommand> DequeueAsync(CancellationToken cancellationToken)
    {
      await _signal.WaitAsync(cancellationToken);
      _workItems.TryDequeue(out var workItem);

      return workItem;
    }
  }
}
