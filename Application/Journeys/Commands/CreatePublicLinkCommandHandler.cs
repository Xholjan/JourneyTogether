using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreatePublicLinkCommandHandler : IRequestHandler<CreatePublicLinkCommand, string>
    {
        private readonly IShareRepository _shareRepo;
        private readonly IUserRepository _userRepo;

        public CreatePublicLinkCommandHandler(IShareRepository shareRepo, IUserRepository userRepo)
        {
            _shareRepo = shareRepo;
            _userRepo = userRepo;
        }

        public async Task<string> Handle(CreatePublicLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var token = await _shareRepo.CreatePublicLinkAsync(request.JourneyId, user.Id, cancellationToken);

            return $"?pj={token}";
        }
    }
}
