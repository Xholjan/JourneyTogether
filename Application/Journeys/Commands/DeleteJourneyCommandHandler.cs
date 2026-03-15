using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class DeleteJourneyCommandHandler : IRequestHandler<DeleteJourneyCommand>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;

        public DeleteJourneyCommandHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(DeleteJourneyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var journey = await _journeyRepo.GetJourneyByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Journey not found");

            if (journey.UserId != user.Id)
                throw new UnauthorizedAccessException("You can only delete your own journeys");

            await _journeyRepo.DeleteJourneyAsync(journey, cancellationToken);
        }
    }
}
