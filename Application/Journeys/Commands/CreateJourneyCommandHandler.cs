using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreateJourneyCommandHandler : IRequestHandler<CreateJourneyCommand, int>
    {
        private readonly IJourneyRepository _repo;

        public CreateJourneyCommandHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<int> Handle(CreateJourneyCommand request, CancellationToken cancellationToken)
        {
            var journey = new Journey
            {
                StartLocation = request.StartLocation,
                StartTime = request.StartTime,
                ArrivalLocation = request.ArrivalLocation,
                ArrivalTime = request.ArrivalTime,
                TransportType = request.TransportType,
                DistanceKm = request.DistanceKm,
                UserId = request.UserId
            };

            var created = await _repo.AddJourneyAsync(journey, cancellationToken);
            return created.Id;
        }
    }
}
