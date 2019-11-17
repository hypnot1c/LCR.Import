using LCR.DataService.Abstractions;
using LCR.TPM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCR.Import.Web.Api.Controllers
{
  public class UsersController : BaseApiController
  {
    public UsersController(
      IDataService dataService,
      ILogger<UsersController> logger
      ) : base(logger)
    {
      this.DataService = dataService;
    }

    public IDataService DataService { get; }

    public async Task<ActionResult<IEnumerable<UserQueryModel>>> GetList()
    {
      var users = await this.DataService.User_GetListAsync();
      return Ok(new { Status = "Ok", result = users.OrderBy(u => u.Lastname + " " + u.Firstname) });
    }
  }
}
