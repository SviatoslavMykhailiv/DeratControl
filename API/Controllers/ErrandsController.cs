using Application.Errands.Commands.CompleteErrand;
using Application.Errands.Commands.DeleteErrand;
using Application.Errands.Commands.GenerateReport;
using Application.Errands.Commands.UpsertErrand;
using Application.Errands.Queries;
using Application.Errands.Queries.GetErrand;
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

    [HttpGet("{errandId}")]
    public async Task<IActionResult> Get(
      [FromRoute]Guid errandId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetErrandQuery(errandId), cancellationToken));

    [HttpGet("{errandId}/report")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReport(
      [FromRoute] Guid errandId,
      CancellationToken cancellationToken) => File(await Mediator.Send(new GenerateReportCommand(errandId), cancellationToken), "application/pdf");

    [HttpPost("complete")]
    public async Task<IActionResult> Complete(
      [FromBody] CompleteErrandCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpDelete("{errandId}")]
    public async Task<IActionResult> Delete(
      [FromRoute]Guid errandId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteErrandCommand(errandId), cancellationToken));
  }
}
