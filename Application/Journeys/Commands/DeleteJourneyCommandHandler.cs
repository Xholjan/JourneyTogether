using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class DeleteJourneyCommandHandler : IRequestHandler<DeleteJourneyCommand>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;
        private readonly IShareRepository _shareRepo;

        public DeleteJourneyCommandHandler(IJourneyRepository journeyRepo, IUserRepository userRepo, IShareRepository shareRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
            _shareRepo = shareRepo;
        }

        public async Task Handle(DeleteJourneyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var journey = await _journeyRepo.GetJourneyByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Journey not found");

            if (journey.UserId != user.Id)
            {
                var share = await _shareRepo.GetSharedJourneyAsync(request.Id, user.Id, cancellationToken);

                if (share == null)
                    throw new CustomException("You can only delete your own journeys");

                await _shareRepo.DeleteSharedJourneyAsync(share.Id, user.Id, cancellationToken);
            }
            else
            {
                await _journeyRepo.DeleteJourneyAsync(journey, cancellationToken);
            }
        }
    }
}
