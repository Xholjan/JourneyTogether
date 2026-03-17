using Api.Extensions;
using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.UserAuth0Id();
            var result = await _mediator.Send(new GetUsersQuery(userId));
            return Ok(result);
        }

        [Authorize]
        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser()
        {
            var userId = User.UserAuth0Id();
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var result = await _mediator.Send(new SyncUserCommand(userId!, email!));

            return Ok(result);
        }
    }
}
