using Domain.Entities;

namespace Application.Interfaces
{
    public interface IShareRepository
    {
        Task ShareJourneyAsync(int journeyId, int sharedByUserId, List<int> userIds, CancellationToken cancellationToken);
        Task<Guid> CreatePublicLinkAsync(int journeyId, int userId, CancellationToken cancellationToken);
        Task<Journey?> GetSharedJourneyByGuidAsync(Guid token, CancellationToken cancellationToken);
        Task<Share?> GetSharedJourneyAsync(int journeyId, int userId, CancellationToken cancellationToken);
        Task DeleteSharedJourneyAsync(int id, int userId, CancellationToken cancellationToken);
    }
}
