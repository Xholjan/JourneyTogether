using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJourneyRepository
    {
        Task<IEnumerable<Journey>> GetJourneysAsync(int userId, CancellationToken cancellationToken);
        Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken);
        Task AddJourneyAsync(Journey journey, CancellationToken cancellationToken);
        Task UpdateJourneyAsync(Journey journey, CancellationToken cancellationToken);
        Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken);
    }
}
