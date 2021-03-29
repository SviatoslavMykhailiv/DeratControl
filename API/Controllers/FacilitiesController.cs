using Application.Facilities.Commands.UpsertFacility;
using Application.Facilities.Queries.GetFacilityDetail;
using Application.Facilities.Queries.GetFacilityList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers {
  [Route("[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  public class FacilitiesController : BaseController {
    [HttpPost]
    public async Task<ActionResult> Upsert([FromBody] UpsertFacilityCommand command) => Ok(await Mediator.Send(command));

    [HttpGet]
    public async Task<ActionResult> GetList() => Ok(await Mediator.Send(new GetFacilityListQuery()));
    
    [HttpGet("{facilityId}")]
    public async Task<ActionResult> GetById([FromRoute] Guid facilityId) => Ok(await Mediator.Send(new GetFacilityDetailQuery(facilityId)));
  }
}
