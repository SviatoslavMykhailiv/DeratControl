using Application.Traps.Commands.DeleteTrap;
using Application.Traps.Commands.UpsertTrap;
using Application.Traps.Queries.GetTrapList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class TrapsController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertTrapCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetTrapListQuery(), cancellationToken));

    [HttpDelete("{trapId}")]
    public async Task<IActionResult> Delete(
      [FromRoute] Guid trapId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteTrapCommand(trapId), cancellationToken));
  }
}
