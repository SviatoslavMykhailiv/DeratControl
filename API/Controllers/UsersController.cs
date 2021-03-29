using Application.Users.Commands.UpsertUser;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  public class UsersController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Upsert([FromBody] UpsertUserCommand command) {
      return Ok(await Mediator.Send(command));
    }
  }
}
