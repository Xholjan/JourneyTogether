using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
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
            var result = await _mediator.Send(new GetUsersQuery());
            return Ok(result);
        }

        [Authorize]
        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser()
        {
            var auth0Id = User.FindFirst("sub")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var result = await _mediator.Send(new SyncUserCommand(auth0Id!, email!));

            return Ok(result);
        }
    }
}
