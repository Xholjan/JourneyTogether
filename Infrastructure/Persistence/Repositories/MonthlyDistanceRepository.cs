using Application.Interfaces;
using Application.Journeys.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Persistence.Repositories
{
    public class MonthlyDistanceRepository : IMonthlyDistanceRepository
    {
        private readonly ApplicationDbContext _context;

        public MonthlyDistanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<MonthlyDistanceModel>> GetQueryableAsync(CancellationToken cancellationToken)
        {
            var query = _context.Set<MonthlyDistance>()
                .AsNoTracking()
                .Select(x => new MonthlyDistanceModel
                {
                    UserId = x.UserId,
                    Year = x.Year,
                    Month = x.Month,
                    TotalDistanceKm = x.TotalDistanceKm
                });

            return await Task.FromResult(query);
        }

        public async Task AddOrUpdateAsync(int userId, DateTime date, decimal distance, CancellationToken cancellationToken)
        {
            var year = date.Year;
            var month = date.Month;

            var record = await _context.MonthlyDistances.FirstOrDefaultAsync(x => x.UserId == userId && x.Year == year && x.Month == month, cancellationToken);

            if (record == null)
            {
                _context.MonthlyDistances.Add(new MonthlyDistance
                {
                    UserId = userId,
                    Year = year,
                    Month = month,
                    TotalDistanceKm = distance
                });
            }
            else
            {
                record.TotalDistanceKm += distance;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(int userId, DateTime date, decimal distance, CancellationToken cancellationToken)
        {
            var record = await _context.MonthlyDistances.FirstOrDefaultAsync(x => x.UserId == userId &&
                                          x.Year == date.Year &&
                                          x.Month == date.Month,
                                          cancellationToken);

            if (record != null)
            {
                record.TotalDistanceKm -= distance;

                if (record.TotalDistanceKm <= 0)
                    _context.MonthlyDistances.Remove(record);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task MoveAsync(int oldUserId, DateTime oldDate, decimal oldDistance, int newUserId, DateTime newDate, decimal newDistance, CancellationToken cancellationToken)
        {
            await RemoveAsync(oldUserId, oldDate, oldDistance, cancellationToken);
            await AddOrUpdateAsync(newUserId, newDate, newDistance, cancellationToken);
        }
    }
}
