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
            var journey = await _mediator.Send(new GetJourneyByIdQuery { Id = id });
            if (journey == null) return NotFound();
            return Ok(journey);
        }

        [HttpGet]
        public async Task<ActionResult<List<JourneyModel>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = 2;

            var journeys = await _mediator.Send(new GetJourneysPagedQuery { UserId = userId, Page = page, PageSize = pageSize });
            return Ok(journeys.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJourneyCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
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
            var userId = 2;

            await _mediator.Send(new DeleteJourneyCommand { Id = id, UserId = userId });
            return NoContent();
        }

        [HttpPost("{id}/share")]
        public async Task<IActionResult> Share(int id, [FromBody] List<int> userIds)
        {
            var userId = 2;

            await _mediator.Send(new ShareJourneyCommand
            {
                JourneyId = id,
                SharedByUserId = userId,
                UserIds = userIds
            });

            return NoContent();
        }

        [HttpPost("{id}/public-link")]
        public async Task<IActionResult> PublicLink(int id)
        {
            var userId = int.Parse(User.FindFirst("sub")!.Value);

            var url = await _mediator.Send(new CreatePublicLinkCommand
            {
                JourneyId = id,
                UserId = userId
            });

            return Ok(url);
        }

        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> AddFavorite(int id)
        {
            var userId = 2;

            await _mediator.Send(new AddFavoriteCommand
            {
                JourneyId = id,
                UserId = userId
            });

            return NoContent();
        }

        [HttpDelete("{id}/favorite")]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var userId = 2;

            await _mediator.Send(new RemoveFavouriteCommand
            {
                JourneyId = id,
                UserId = userId
            });

            return NoContent();
        }
    }
}
