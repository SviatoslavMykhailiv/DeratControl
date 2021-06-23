using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "PROVIDER")]
    public class SettingsController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SaveSettings(
            [FromServices] IUserManagerService userManagerService,
            [FromServices] ICurrentUserProvider currentUserProvider,
            [FromServices] IFileStorage fileStorage,
            [FromForm]IFormFile signature, 
            [FromForm]string providerName) 
        {
            using var memoryStream = new MemoryStream();
            await signature.CopyToAsync(memoryStream);
            await fileStorage.SaveFile(Path.Combine("signatures", currentUserProvider.User.UserId.ToString()), memoryStream.ToArray());
            await userManagerService.UpdateProviderName(currentUserProvider.User.UserId, providerName);

            return Ok();
        }

        [HttpGet("signature")]
        public async Task<IActionResult> GetSignature(
            [FromServices] ICurrentUserProvider currentUserProvider,
            [FromServices] IFileStorage fileStorage) => File(await fileStorage.ReadFile(Path.Combine("signatures", currentUserProvider.User.UserId.ToString())), $"image/png");
    }
}
