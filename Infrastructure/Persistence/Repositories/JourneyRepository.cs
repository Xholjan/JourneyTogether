using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public JourneyRepository(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Journey>> GetJourneysAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Journeys.Where(j => j.UserId == userId).Include(j => j.User).ToListAsync(cancellationToken);
        }

        public async Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Journeys.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }

        public async Task AddJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Add(journey);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JourneyCreated(journey), cancellationToken);
        }

        public async Task UpdateJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Update(journey);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JourneyUpdated(journey), cancellationToken);
        }

        public async Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Remove(journey);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JourneyDeleted(journey), cancellationToken);
        }
    }
}
