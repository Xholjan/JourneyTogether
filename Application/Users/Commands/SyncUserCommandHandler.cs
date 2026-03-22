using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands
{
    public class SyncUserCommandHandler : IRequestHandler<SyncUserCommand, Models.UserStatus>
    {
        private readonly IUserRepository _userRepository;

        public SyncUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Models.UserStatus> Handle(SyncUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Auth0Id))
                throw new CustomException("Try login again.");

            var user = await _userRepository.CheckAsync(request.Auth0Id, cancellationToken);

            if (user == null)
            {
                user = new User
                {
                    Auth0Id = request.Auth0Id,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = request.Role,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddUserAsync(user, cancellationToken);
            }
            else
            {
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Role = request.Role;
                await _userRepository.UpdateUserAsync(user.Id, user, false, cancellationToken);
            }

            return (Models.UserStatus)user.Status;
        }
    }
}
