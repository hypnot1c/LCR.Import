using System.Threading;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public interface IBackgroundQueue<T> where T : class
  {
    void Enqueue(T workItem);

    Task<T> DequeueAsync(CancellationToken cancellationToken);
  }
}
