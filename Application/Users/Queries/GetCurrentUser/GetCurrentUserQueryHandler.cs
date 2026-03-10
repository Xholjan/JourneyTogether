using Application.Users.Interfaces;
using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserModel>
    {
        private readonly IUserRepository _repo;

        public GetCurrentUserQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserModel> Handle(GetCurrentUserQuery request, CancellationToken ct)
        {
            var user = await _repo.GetByAuth0Id(request.Auth0Id, ct);

            if (user == null)
                throw new Exception("User not found");

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
