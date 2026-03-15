using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneysPagedQueryHandler : IRequestHandler<GetJourneysPagedQuery, IEnumerable<JourneyModel>>
    {
        private readonly IJourneyRepository _repo;

        public GetJourneysPagedQueryHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<IEnumerable<JourneyModel>> Handle(GetJourneysPagedQuery request, CancellationToken cancellationToken)
        {
            var allJourneys = await _repo.GetJourneysAsync(request.UserId, cancellationToken);
            return allJourneys.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(j => new JourneyModel
            {
                Id = j.Id,
                StartLocation = j.StartLocation,
                StartTime = j.StartTime,
                ArrivalLocation = j.ArrivalLocation,
                ArrivalTime = j.ArrivalTime,
                TransportType = (TransportType)j.TransportType,
                DistanceKm = j.DistanceKm
            });
        }
    }
}
