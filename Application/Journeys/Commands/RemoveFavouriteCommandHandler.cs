using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class RemoveFavouriteCommandHandler : IRequestHandler<RemoveFavouriteCommand>
    {
        private readonly IFavouriteRepository _repo;

        public RemoveFavouriteCommandHandler(IFavouriteRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(RemoveFavouriteCommand request, CancellationToken cancellationToken)
        {
            await _repo.RemoveFavouriteAsync(request.JourneyId, request.UserId, cancellationToken);
        }
    }
}
