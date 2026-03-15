using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreateJourneyCommandHandler : IRequestHandler<CreateJourneyCommand>
    {
        private readonly IJourneyRepository _repo;

        public CreateJourneyCommandHandler(IJourneyRepository repo) => _repo = repo;

        public async Task Handle(CreateJourneyCommand request, CancellationToken cancellationToken)
        {
            var journey = new Journey
            {
                StartLocation = request.StartLocation,
                StartTime = request.StartTime,
                ArrivalLocation = request.ArrivalLocation,
                ArrivalTime = request.ArrivalTime,
                TransportType = (TransportType)request.TransportType,
                DistanceKm = request.DistanceKm,
                UserId = request.UserId
            };

            await _repo.AddJourneyAsync(journey, cancellationToken);
        }
    }
}
