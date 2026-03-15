using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
    {
        private readonly IFavouriteRepository _repo;

        public AddFavoriteCommandHandler(IFavouriteRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _repo.AddFavouriteAsync(request.JourneyId, request.UserId, cancellationToken);
        }
    }
}
