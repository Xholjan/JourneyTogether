using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneysPagedQueryHandler : IRequestHandler<GetJourneysPagedQuery, PagedModel<JourneyModel>>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;

        public GetJourneysPagedQueryHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task<PagedModel<JourneyModel>> Handle(GetJourneysPagedQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var allJourneys = await _journeyRepo.GetJourneysAsync(user.Id, cancellationToken);

            var result = allJourneys.Select(j => new JourneyModel
            {
                Id = j.Id,
                StartLocation = j.StartLocation,
                StartTime = j.StartTime,
                ArrivalLocation = j.ArrivalLocation,
                ArrivalTime = j.ArrivalTime,
                TransportType = (TransportType)j.TransportType,
                DistanceKm = j.DistanceKm
            });

            return new PagedModel<JourneyModel>(result, request.Page, request.PageSize);
        }
    }
}
