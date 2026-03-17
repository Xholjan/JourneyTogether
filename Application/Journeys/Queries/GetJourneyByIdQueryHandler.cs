using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByIdQueryHandler : IRequestHandler<GetJourneyByIdQuery, JourneyModel?>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;
        public GetJourneyByIdQueryHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task<JourneyModel?> Handle(GetJourneyByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            return await _journeyRepo.GetJourneyWithFavouritesByIdAsync(request.Id, user.Id, cancellationToken);
        }
    }
}
