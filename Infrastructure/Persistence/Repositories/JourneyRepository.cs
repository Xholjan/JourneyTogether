using Application.Interfaces;
using Application.Journeys.Models;
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
            return await _context.Journeys
                .Include(j => j.User)
                .Where(j => j.UserId == userId || _context.Shares.Any(s => s.JourneyId == j.Id && s.SharedWithUserId == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task<Journey?> GetJourneyByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Journeys.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }

        public async Task<JourneyModel?> GetJourneyWithFavouritesByIdAsync(int id, int userId, CancellationToken cancellationToken)
        {
            var journey = await _context.Journeys
                .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

            if (journey == null) return null;

            var isFavourite = await _context.Favourites
                .AnyAsync(f => f.JourneyId == id && f.UserId == userId, cancellationToken);

            return new JourneyModel
            {
                Id = journey.Id,
                StartLocation = journey.StartLocation,
                StartTime = journey.StartTime,
                ArrivalLocation = journey.ArrivalLocation,
                ArrivalTime = journey.ArrivalTime,
                TransportType = (Application.Journeys.Models.TransportType)journey.TransportType,
                DistanceKm = journey.DistanceKm,
                IsFavourite = isFavourite
            };
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
