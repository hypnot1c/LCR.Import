using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Resources
{
  public interface IProccessFileCommandService<T> where T : ICommand
  {
    Task OnNextAsync(T command);
  }
}
