using Api.Extensions;
using Application.Journeys.Commands;
using Application.Journeys.Models;
using Application.Journeys.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/journeys")]
    public class JourneysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JourneysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JourneyModel>> GetById(int id)
        {
            var journey = await _mediator.Send(new GetJourneyByIdQuery(id));
            if (journey == null) return NotFound();
            return Ok(journey);
        }

        [HttpGet]
        public async Task<ActionResult<PagedModel<JourneyModel>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = User.UserAuth0Id();

            var journeys = await _mediator.Send(new GetJourneysPagedQuery(userId, page, pageSize));
            return Ok(journeys);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJourneyCommand command)
        {
            var userId = User.UserAuth0Id();
            command.UserId = userId;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJourneyCommand command)
        {
            if (id != command.Id) return BadRequest();

            var userId = User.UserAuth0Id();
            command.UserId = userId;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.UserAuth0Id();

            await _mediator.Send(new DeleteJourneyCommand(id, userId));
            return NoContent();
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> Share(int id, [FromBody] List<int> userIds)
        {
            var userId = User.UserAuth0Id();

            await _mediator.Send(new ShareJourneyCommand(id, userId, userIds));
            return NoContent();
        }

        [HttpPost("{id}/public-link")]
        public async Task<IActionResult> PublicLink(int id)
        {
            var userId = User.UserAuth0Id();

            var url = await _mediator.Send(new CreatePublicLinkCommand(id, userId));
            return Ok(url);
        }

        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> AddFavorite(int id)
        {
            var userId = User.UserAuth0Id();

            await _mediator.Send(new AddFavoriteCommand(id, userId));
            return NoContent();
        }

        [HttpDelete("{id}/favorite")]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var userId = User.UserAuth0Id();

            await _mediator.Send(new RemoveFavouriteCommand(id, userId));
            return NoContent();
        }
    }
}
