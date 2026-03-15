using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class RemoveFavouriteCommandHandler : IRequestHandler<RemoveFavouriteCommand>
    {
        private readonly IFavouriteRepository _favouriteRepo;
        private readonly IUserRepository _userRepo;

        public RemoveFavouriteCommandHandler(IFavouriteRepository favouriteRepo, IUserRepository userRepo)
        {
            _favouriteRepo = favouriteRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(RemoveFavouriteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            await _favouriteRepo.RemoveFavouriteAsync(request.JourneyId, user.Id, cancellationToken);
        }
    }
}
