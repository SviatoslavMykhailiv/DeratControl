using Application.Common.Interfaces;
using Application.Perimeters.Commands.DeletePerimeter;
using Application.Perimeters.Commands.UpsertPerimeter;
using Application.Perimeters.Queries.GetPerimeterDetail;
using Application.Perimeters.Queries.GetPerimeterSchemeImage;
using Application.QRCodes.GeneratePointQRCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class PerimetersController : BaseController {
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    [HttpPost]
    public async Task<IActionResult> Upsert(
          [FromBody] UpsertPerimeterCommand command,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    [HttpGet("{perimeterId}")]
    public async Task<IActionResult> Get(
      [FromRoute] Guid perimeterId,
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetPerimeterDetailQuery(perimeterId), cancellationToken));

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    [HttpPost("qrs")]
    public async Task<IActionResult> GenerateQRCodeList(
      [FromBody] GeneratePointQRListCommand command,
      CancellationToken cancellationToken) => File(await Mediator.Send(command, cancellationToken), "application/pdf");

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    [HttpDelete("{perimeterId}")]
    public async Task<IActionResult> Delete(
      [FromRoute] Guid perimeterId,
      CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeletePerimeterCommand(perimeterId), cancellationToken));

    [HttpGet("schemes/{fileName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSchemeImage([FromRoute] string fileName, [FromServices] IFileStorage fileStorage) => File(await fileStorage.ReadFile(Path.Combine("perimeters", "schemes", fileName)), $"image/{fileName.Split('.')[1]}");

    [HttpGet("{perimeterId}/scheme")]
    [AllowAnonymous]
    public async Task<IActionResult> GetScheme([FromRoute] Guid perimeterId) => File(await Mediator.Send(new GetPerimeterSchemeImageQuery(perimeterId)), $"image/png");
  }
}
