using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands.SyncUser
{
    public record SyncUserCommand(string Auth0Id, string Email) : IRequest<UserModel>;
}
