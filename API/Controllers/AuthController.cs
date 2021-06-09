using Application.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] AuthCommand command, CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));
    }
}
