using Application.Perimeters.Commands.DeletePerimeter;
using Application.Perimeters.Commands.UpsertPerimeter;
using Application.Perimeters.Queries.GetPerimeterDetail;
using Application.QRCodes.GeneratePointQRCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class PerimetersController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertPerimeterCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpGet("{perimeterId}")]
    public async Task<IActionResult> Get(
      [FromRoute] Guid perimeterId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetPerimeterDetailQuery(perimeterId), cancellationToken));

    [HttpPost("qrs")]
    public async Task<IActionResult> GenerateQRCodeList(
      [FromBody] GeneratePointQRListCommand command, 
      CancellationToken cancellationToken) => File(await Mediator.Send(command, cancellationToken), "application/pdf");

    [HttpDelete("{perimeterId}")]
    public async Task<IActionResult> Delete(
      [FromRoute] Guid perimeterId, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeletePerimeterCommand(perimeterId), cancellationToken));
  }
}
