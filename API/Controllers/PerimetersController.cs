using Application.Perimeters.Commands.UpsertPerimeter;
using Application.Perimeters.Queries.GetPerimeterDetail;
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
  }
}
