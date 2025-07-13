using Application.Errands.Commands.CompleteErrand;
using Application.Errands.Commands.DeleteErrand;
using Application.Errands.Commands.GenerateReport;
using Application.Errands.Commands.UpsertErrand;
using Application.Errands.Queries;
using Application.Errands.Queries.GetErrand;
using Application.Errands.Queries.GetErrandHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrandsController : BaseController
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpPost]
        public async Task<IActionResult> Upsert(
          [FromBody] UpsertErrandCommand command,
          CancellationToken cancellationToken = default) => Ok(await Mediator.Send(command, cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER,EMPLOYEE")]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetErrandListQuery query, CancellationToken cancellationToken = default) => Ok(await Mediator.Send(query, cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER,EMPLOYEE")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryList(
          [FromQuery] Guid? facilityId,
          [FromQuery] Guid? employeeId,
          [FromQuery] int skip = 0,
          [FromQuery] int take = 10,
          CancellationToken cancellationToken = default) => Ok(await Mediator.Send(new GetErrandHistoryQuery(facilityId, employeeId, skip, take), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER,EMPLOYEE")]
        [HttpGet("{errandId}")]
        public async Task<IActionResult> Get(
          [FromRoute] Guid errandId,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetErrandQuery(errandId), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER,CUSTOMER")]
        [HttpGet("{errandId}/report")]
        public async Task<IActionResult> GetReport(
          [FromRoute] Guid errandId,
          CancellationToken cancellationToken = default) => File(await Mediator.Send(new GenerateReportCommand(errandId), cancellationToken), "application/pdf");

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "EMPLOYEE,PROVIDER")]
        [HttpPost("complete")]
        public async Task<IActionResult> Complete(
          [FromBody] CompleteErrandCommand command,
          CancellationToken cancellationToken = default)
        {
            await Mediator.Send(command, cancellationToken);

            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpDelete("{errandId}")]
        public async Task<IActionResult> Delete(
          [FromRoute] Guid errandId,
          CancellationToken cancellationToken = default)
        {
            await Mediator.Send(new DeleteErrandCommand(errandId), cancellationToken);

            return Ok();
        }
    }
}
