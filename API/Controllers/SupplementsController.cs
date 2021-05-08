using Application.Supplements.Commands.DeleteSupplement;
using Application.Supplements.Commands.UpsertSupplement;
using Application.Supplements.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class SupplementsController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertSupplementCommand command,
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetSupplementListQuery(), cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(
      [FromRoute] Guid supplementId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteSupplementCommand(supplementId), cancellationToken));
  }
}
