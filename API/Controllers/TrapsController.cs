using Application.Traps.Commands.DeleteTrap;
using Application.Traps.Commands.UpsertTrap;
using Application.Traps.Queries.GetTrap;
using Application.Traps.Queries.GetTrapList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TrapsController : BaseController
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpPost]
        public async Task<IActionResult> Upsert(
          [FromBody] UpsertTrapCommand command,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetTrapListQuery(), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpGet("{trapId}")]
        public async Task<IActionResult> Get([FromRoute] Guid trapId, CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetTrapQuery(trapId), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpDelete("{trapId}")]
        public async Task<IActionResult> Delete(
          [FromRoute] Guid trapId,
          CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteTrapCommand(trapId), cancellationToken);

            return Ok();
        }
    }
}
