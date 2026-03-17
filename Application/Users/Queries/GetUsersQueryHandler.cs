using Application.Interfaces;
using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
    {
        private readonly IUserRepository _userRepo;

        public GetUsersQueryHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var users = await _userRepo.GetAllUsersAsync(cancellationToken);

            return users.Where(u => u.Id != user.Id).Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).ToList();
        }
    }
}
