using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class UpdateJourneyCommandHandler : IRequestHandler<UpdateJourneyCommand, Unit>
    {
        private readonly IJourneyRepository _repo;

        public UpdateJourneyCommandHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<Unit> Handle(UpdateJourneyCommand request, CancellationToken cancellationToken)
        {
            var journey = await _repo.GetJourneyByIdAsync(request.Id, cancellationToken);

            if (journey is null)
                throw new KeyNotFoundException("Journey not found");

            if (journey.UserId != request.UserId)
                throw new UnauthorizedAccessException("You can only update your own journeys");

            journey.StartLocation = request.StartLocation;
            journey.StartTime = request.StartTime;
            journey.ArrivalLocation = request.ArrivalLocation;
            journey.ArrivalTime = request.ArrivalTime;
            journey.TransportType = request.TransportType;
            journey.DistanceKm = request.DistanceKm;

            await _repo.UpdateJourneyAsync(journey, cancellationToken);

            return Unit.Value;
        }
    }
}
