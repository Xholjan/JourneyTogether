using Application.Journeys.Models;
using Application.Journeys.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JourneyModel>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid token))
                return BadRequest("Invalid GUID");

            var journey = await _mediator.Send(new GetJourneyByPublicIdQuery(token));
            if (journey == null) return NotFound();
            return Ok(journey);
        }
    }
}
