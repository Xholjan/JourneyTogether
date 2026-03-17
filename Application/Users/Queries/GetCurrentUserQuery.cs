using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries
{
    public class GetCurrentUserQuery : IRequest<UserModel>
    {
        public string Auth0Id { get; }

        public GetCurrentUserQuery(string auth0Id)
        {
            Auth0Id = auth0Id;
        }
    }
}
