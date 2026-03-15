using Application.Interfaces.Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class ShareJourneyCommandHandler : IRequestHandler<ShareJourneyCommand>
    {
        private readonly IShareRepository _repo;

        public ShareJourneyCommandHandler(IShareRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(ShareJourneyCommand request, CancellationToken cancellationToken)
        {
            await _repo.ShareJourneyAsync(request.JourneyId, request.SharedByUserId, request.UserIds, cancellationToken);
        }
    }
}
