using Application.Exceptions;
using Application.Interfaces;
using Application.Users.Events;
using MediatR;

namespace Application.Users.Commands
{
    public class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public UpdateUserStatusHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            var loggedUser = await _userRepository.GetByAuth0Id(request.UserId, cancellationToken);

            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new CustomException("User not found");

            var oldStatus = user.Status;
            user.Status = (Domain.Entities.UserStatus)request.NewStatus;

            await _userRepository.UpdateUserAsync(loggedUser.Id, user, true, cancellationToken);

            await _mediator.Publish(new UserStatusChanged(user, oldStatus, (Domain.Entities.UserStatus)request.NewStatus), cancellationToken);
        }
    }
}
