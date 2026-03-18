using Api.Extensions;
using Application.Users.Commands;
using Application.Users.Models;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("sync")]
        public async Task<ActionResult<UserStatus>> SyncUser()
        {
            var userId = User.UserAuth0Id();

            var ns = "https://journeytogether.com/";
            var email = User.FindFirst(ns + "email")?.Value ?? "";
            var firstName = User.FindFirst(ns + "given_name")?.Value ?? "";
            var lastName = User.FindFirst(ns + "family_name")?.Value ?? "";
            var role = User.FindFirst(ns + "roles")?.Value ?? "User";

            var command = new SyncUserCommand(userId, email, firstName, lastName, role);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
