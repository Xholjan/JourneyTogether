using Application.Journeys.Models;

namespace Application.Interfaces
{
    public interface IMonthlyDistanceRepository
    {
        Task<IQueryable<MonthlyDistanceModel>> GetQueryableAsync(CancellationToken cancellationToken);
        Task AddOrUpdateAsync(int userId, DateTime date, decimal distance, CancellationToken cancellationToken);
        Task RemoveAsync(int userId, DateTime date, decimal distance, CancellationToken cancellationToken);
        Task MoveAsync(int oldUserId, DateTime oldDate, decimal oldDistance, int newUserId, DateTime newDate, decimal newDistance, CancellationToken cancellationToken);
    }
}
