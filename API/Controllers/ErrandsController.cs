using Application.Errands.Commands.CompleteErrand;
using Application.Errands.Commands.DeleteErrand;
using Application.Errands.Commands.UpsertErrand;
using Application.Errands.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class ErrandsController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertErrandCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetErrandListQuery(), cancellationToken));

    [HttpPost("complete")]
    public async Task<IActionResult> Complete(
      [FromBody] CompleteErrandCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(
      [FromRoute]Guid errandId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteErrandCommand(errandId), cancellationToken));
  }
}
