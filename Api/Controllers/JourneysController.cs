using Application.Journeys.Commands;
using Application.Journeys.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/journeys")]
    public class JourneysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JourneysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJourneyCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Journey>> GetById(int id)
        {
            var journey = await _mediator.Send(new GetJourneyByIdQuery(id));
            if (journey == null) return NotFound();
            return Ok(journey);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Journey>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = 2;
            var journeys = await _mediator.Send(new GetJourneysPagedQuery(userId, page, pageSize));
            return Ok(journeys);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJourneyCommand command)
        {
            if (id != command.Id) return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _mediator.Send(new DeleteJourneyCommand(id, userId));
            return NoContent();
        }
    }
}
