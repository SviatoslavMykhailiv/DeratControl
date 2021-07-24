using Application.Points.Queries.PointReviewHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PointsController : BaseController
    {
        [HttpGet("{pointId}/history")]
        public async Task<IActionResult> GetPointReviewHistory([FromRoute]Guid pointId) 
        {
            return Ok(await Mediator.Send(new PointReviewHistoryQuery(pointId)));
        }
    }
}
