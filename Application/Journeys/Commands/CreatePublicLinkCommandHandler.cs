using Application.Interfaces.Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreatePublicLinkCommandHandler : IRequestHandler<CreatePublicLinkCommand, string>
    {
        private readonly IShareRepository _repo;

        public CreatePublicLinkCommandHandler(IShareRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(CreatePublicLinkCommand request, CancellationToken cancellationToken)
        {
            return await _repo.CreatePublicLinkAsync(request.JourneyId, request.UserId, cancellationToken);
        }
    }
}
