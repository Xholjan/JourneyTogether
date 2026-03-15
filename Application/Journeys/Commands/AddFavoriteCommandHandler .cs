using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
    {
        private readonly IFavouriteRepository _favouriteRepo;
        private readonly IUserRepository _userRepo;

        public AddFavoriteCommandHandler(IFavouriteRepository favouriteRepo, IUserRepository userRepo)
        {
            _favouriteRepo = favouriteRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            await _favouriteRepo.AddFavouriteAsync(request.JourneyId, user.Id, cancellationToken);
        }
    }
}
