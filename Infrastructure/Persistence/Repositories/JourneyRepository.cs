using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly ApplicationDbContext _context;

        public JourneyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Journey> AddJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Add(journey);
            await _context.SaveChangesAsync(cancellationToken);
            return journey;
        }

        public async Task UpdateJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Update(journey);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Journey>> GetJourneysAsync(CancellationToken cancellationToken)
        {
            return await _context.Journeys.Include(j => j.User).ToListAsync(cancellationToken);
        }

        public async Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Journeys.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Journey>> GetJourneysByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Journeys.Include(j => j.User).Where(j => j.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Remove(journey);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
