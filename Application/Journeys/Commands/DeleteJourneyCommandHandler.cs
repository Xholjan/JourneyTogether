using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class DeleteJourneyCommandHandler : IRequestHandler<DeleteJourneyCommand, Unit>
    {
        private readonly IJourneyRepository _repo;

        public DeleteJourneyCommandHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<Unit> Handle(DeleteJourneyCommand request, CancellationToken cancellationToken)
        {
            var journey = await _repo.GetJourneyByIdAsync(request.Id, cancellationToken);

            if (journey is null)
                throw new KeyNotFoundException("Journey not found");

            if (journey.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only delete your own journeys");

            await _repo.DeleteJourneyAsync(journey, cancellationToken);

            return Unit.Value;
        }
    }
}
