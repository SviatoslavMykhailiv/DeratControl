using Application.Supplements.Commands.UpsertSupplement;
using Application.Supplements.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  public class SupplementsController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Upsert([FromBody] UpsertSupplementCommand command) => Ok(await Mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult> GetList() => Ok(await Mediator.Send(new GetSupplementListQuery()));
  }
}
