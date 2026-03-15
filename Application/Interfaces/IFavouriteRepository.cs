using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFavouriteRepository
    {
        Task AddFavouriteAsync(int journeyId, int userId, CancellationToken cancellationToken);
        Task RemoveFavouriteAsync(int journeyId, int userId, CancellationToken cancellationToken);
        Task<List<User>> GetUsersFavouritingJourneyAsync(int journeyId, CancellationToken cancellationToken);
    }
}
