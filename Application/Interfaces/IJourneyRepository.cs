using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJourneyRepository
    {
        Task<Journey> AddJourneyAsync(Journey journey, CancellationToken cancellationToken);
        Task<IEnumerable<Journey>> GetJourneysAsync(CancellationToken cancellationToken);
        Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Journey>> GetJourneysByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken);
        Task UpdateJourneyAsync(Journey journey, CancellationToken cancellationToken);
    }
}
