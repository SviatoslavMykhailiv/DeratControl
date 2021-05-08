using Application.Facilities.Commands.UpsertFacility;
using Application.Facilities.Queries.GetFacilityDetail;
using Application.Facilities.Queries.GetFacilityList;
using Application.QRCodes.GenerateFacilityQRCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class FacilitiesController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertFacilityCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpPost("qrs")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateQRCodeList(
      [FromBody] GenerateFacilityQRCodesCommand command,
      CancellationToken cancellationToken) => File(await Mediator.Send(command, cancellationToken), "application/pdf");

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetFacilityListQuery(), cancellationToken));

    [HttpGet("{facilityId}")]
    public async Task<IActionResult> Get(
      [FromRoute] Guid facilityId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetFacilityDetailQuery(facilityId), cancellationToken));
  }
}
