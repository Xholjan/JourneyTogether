using Api.Extensions;
using Application.Journeys.Queries;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Admin");
        }

        [HttpGet("journeys")]
        public async Task<IActionResult> GetJourneys([FromQuery] GetAdminJourneysQuery query, CancellationToken cancellationToken)
        {
            var userId = User.UserAuth0Id();
            query.UserId = userId;
            var paged = await _mediator.Send(query);
            Response.Headers.Append("X-TotalCount", paged.TotalItems.ToString());
            return Ok(paged);
        }

        [HttpGet("statistics/monthly-distance")]
        public async Task<IActionResult> GetMonthlyDistance([FromQuery] GetMonthlyDistanceQuery query)
        {
            var paged = await _mediator.Send(query);
            Response.Headers.Append("X-Total-Count", paged.TotalItems.ToString());
            return Ok(paged);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateUserStatusCommand command)
        {
            var userId = User.UserAuth0Id();
            command.UserId = userId;
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
