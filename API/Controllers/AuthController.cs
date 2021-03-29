using Application.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  public class AuthController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Auth([FromBody] AuthCommand command) => Ok(await Mediator.Send(command));
  }
}
