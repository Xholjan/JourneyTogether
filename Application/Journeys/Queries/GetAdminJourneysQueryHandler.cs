using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetAdminJourneysQueryHandler : IRequestHandler<GetAdminJourneysQuery, PagedModel<JourneyModel>>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;
        public GetAdminJourneysQueryHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task<PagedModel<JourneyModel>> Handle(GetAdminJourneysQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var result = await _journeyRepo.GetAdminJourneysAsync(request, cancellationToken);

            return new PagedModel<JourneyModel>(result, request.Page, request.PageSize);
        }
    }
}
