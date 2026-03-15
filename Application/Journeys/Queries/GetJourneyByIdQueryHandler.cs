using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByIdQueryHandler : IRequestHandler<GetJourneyByIdQuery, JourneyModel?>
    {
        private readonly IJourneyRepository _repo;
        public GetJourneyByIdQueryHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<JourneyModel?> Handle(GetJourneyByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetJourneyByIdAsync(request.Id, cancellationToken);

            return result is null ? null : new JourneyModel
            {
                Id = result.Id,
                StartLocation = result.StartLocation,
                StartTime = result.StartTime,
                ArrivalLocation = result.ArrivalLocation,
                ArrivalTime = result.ArrivalTime,
                TransportType = (TransportType)result.TransportType,
                DistanceKm = result.DistanceKm
            };
        }
    }
}
