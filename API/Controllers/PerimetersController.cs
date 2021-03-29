using Application.Perimeters.Commands.UpsertPerimeter;
using Application.Perimeters.Queries.GetPerimeterDetail;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  public class PerimetersController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Upsert([FromBody] UpsertPerimeterCommand command) => Ok(await Mediator.Send(command));

    [HttpGet("{perimeterId}")]
    public async Task<ActionResult> GetById([FromRoute] Guid perimeterId) => Ok(await Mediator.Send(new GetPerimeterDetailQuery(perimeterId)));
  }
}
