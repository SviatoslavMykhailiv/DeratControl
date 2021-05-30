using Application.Common.Interfaces;
using Application.Supplements.Commands.DeleteSupplement;
using Application.Supplements.Commands.UpsertSupplement;
using Application.Supplements.Queries;
using Application.Supplements.Queries.GetSupplement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplementsController : BaseController
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpPost]
        public async Task<IActionResult> Upsert(
          [FromBody] UpsertSupplementCommand command,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(command, cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpGet]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetSupplementListQuery(), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpGet("{supplementId}")]
        public async Task<IActionResult> Get([FromRoute] Guid supplementId, CancellationToken cancellationToken) => Ok(await Mediator.Send(new GetSupplementQuery(supplementId), cancellationToken));

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
        [HttpDelete("{supplementId}")]
        public async Task<IActionResult> Delete(
          [FromRoute] Guid supplementId,
          CancellationToken cancellationToken) => Ok(await Mediator.Send(new DeleteSupplementCommand(supplementId), cancellationToken));

        [HttpGet("certificate/{fileName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCertificate(
          [FromRoute] string fileName,
          [FromServices] IFileStorage fileStorage) => File(await fileStorage.ReadFile(Path.Combine("supplements", "certificate", fileName)), $"image/{fileName.Split('.')[1]}");
    }
}
