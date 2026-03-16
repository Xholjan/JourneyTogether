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
            if (request.UserId == null)
                throw new Exception("User not found");

            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

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
