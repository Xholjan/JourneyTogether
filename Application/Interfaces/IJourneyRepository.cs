using Application.Journeys.Models;
using Application.Journeys.Queries;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJourneyRepository
    {
        Task<IEnumerable<Journey>> GetJourneysAsync(int userId, CancellationToken cancellationToken);
        Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken);
        Task<JourneyModel?> GetJourneyWithFavouritesByIdAsync(int id, int userId, CancellationToken cancellationToken);
        Task<List<JourneyModel>> GetAdminJourneysAsync(GetAdminJourneysQuery request, CancellationToken cancellationToken);
        Task AddJourneyAsync(Journey journey, CancellationToken cancellationToken);
        Task UpdateJourneyAsync(Journey journey, bool inCreate, CancellationToken cancellationToken);
        Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken);
    }
}
