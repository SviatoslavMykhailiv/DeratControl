using Application.Traps.Commands.UpsertTrap;
using Application.Traps.Queries.GetTrapList;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  public class TrapsController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Upsert([FromBody] UpsertTrapCommand command) => Ok(await Mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult> GetList() => Ok(await Mediator.Send(new GetTrapListQuery()));
  }
}
