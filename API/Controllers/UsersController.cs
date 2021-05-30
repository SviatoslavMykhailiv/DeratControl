using Application.Users.Commands.SetUserAvailability;
using Application.Users.Commands.UpsertUser;
using Application.Users.Queries.GetAvailableEmployeeList;
using Application.Users.Queries.GetEmployeeList;
using Application.Users.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    public class UsersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Upsert(
          [FromBody] UpsertUserCommand command,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployeeList([FromQuery] GetEmployeeListQuery query) => Ok(await Mediator.Send(query));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId) => Ok(await Mediator.Send(new GetUserQuery(userId)));


        [HttpPost("availability")]
        public async Task<IActionResult> SetUserActive([FromBody] SetUserAvailabilityCommand command) => Ok(await Mediator.Send(command));

        [HttpGet("employees/available")]
        public async Task<IActionResult> GetAvailableEmployees([FromQuery] GetAvailableEmployeeListQuery command) => Ok(await Mediator.Send(command));
    }
}
