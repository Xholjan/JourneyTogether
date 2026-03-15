using Application.Interfaces;
using Application.Interfaces.Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class ShareJourneyCommandHandler : IRequestHandler<ShareJourneyCommand>
    {
        private readonly IShareRepository _shareRepo;
        private readonly IUserRepository _userRepo;

        public ShareJourneyCommandHandler(IShareRepository shareRepo, IUserRepository userRepo)
        {
            _shareRepo = shareRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(ShareJourneyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            await _shareRepo.ShareJourneyAsync(request.JourneyId, user.Id, request.UserIds, cancellationToken);
        }
    }
}
