using Application.Users.Interfaces;
using Application.Users.Models;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.SyncUser
{

    public class SyncUserCommandHandler : IRequestHandler<SyncUserCommand, UserModel>
    {
        private readonly IUserRepository _userRepository;

        public SyncUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserModel> Handle(SyncUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByAuth0Id(request.Auth0Id, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Auth0Id = request.Auth0Id,
                    Email = request.Email
                };

                await _userRepository.AddUserAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);
            }

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
