using Application.Users.Commands.SetUserAvailability;
using Application.Users.Commands.UpsertUser;
using Application.Users.Queries.GetAvailableEmployeeList;
using Application.Users.Queries.GetEmployeeList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class UsersController : BaseController {
    [HttpPost]
    public async Task<IActionResult> Upsert(
      [FromBody] UpsertUserCommand command, 
      CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployeeList([FromQuery] GetEmployeeListQuery query) => Ok(await Mediator.Send(query));

    [HttpPost("availability")]
    public async Task<IActionResult> SetUserActive([FromBody] SetUserAvailabilityCommand command) => Ok(await Mediator.Send(command));

    [HttpGet("employees/available")]
    public async Task<IActionResult> GetAvailableEmployees([FromQuery] GetAvailableEmployeeListQuery command) => Ok(await Mediator.Send(command));
  }
}
