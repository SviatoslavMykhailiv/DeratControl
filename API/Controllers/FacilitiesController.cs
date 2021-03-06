using Application.Facilities.Commands.DeleteFacility;
using Application.Facilities.Commands.UpsertFacility;
using Application.Facilities.Queries.GetFacilityDetail;
using Application.Facilities.Queries.GetFacilityList;
using Application.QRCodes.GenerateFacilityQRCodes;
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
    public class FacilitiesController : BaseController
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpPost]
        public async Task<IActionResult> Upsert(
          [FromBody] UpsertFacilityCommand command,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpPost("qrs")]
        public async Task<IActionResult> GenerateQRCodeList(
          [FromBody] GenerateFacilityQRCodesCommand command,
          CancellationToken cancellationToken) => File(await Mediator.Send(command, cancellationToken), "application/pdf");

        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetFacilityListQuery(), cancellationToken));

        [HttpGet("{facilityId}")]
        public async Task<IActionResult> Get(
          [FromRoute] Guid facilityId,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetFacilityDetailQuery(facilityId), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpDelete("{facilityId}")]
        public async Task<IActionResult> Delete(
          [FromRoute] Guid facilityId,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteFacilityCommand(facilityId), cancellationToken));
    }
}
