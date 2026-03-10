using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(string Auth0Id) : IRequest<UserModel>;
}
