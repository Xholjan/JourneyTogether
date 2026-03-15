using Application.Interfaces;
using Application.Journeys.Events;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Persistence.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public FavouriteRepository(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task AddFavouriteAsync(int journeyId, int userId, CancellationToken cancellationToken)
        {
            var exists = await _context.Favourites.AnyAsync(f => f.JourneyId == journeyId && f.UserId == userId, cancellationToken);

            if (exists)
                return;

            var favorite = new Favourite
            {
                JourneyId = journeyId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Favourites.Add(favorite);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new FavouriteAdded(favorite), cancellationToken);
        }

        public async Task RemoveFavouriteAsync(int journeyId, int userId, CancellationToken cancellationToken)
        {
            var favorite = await _context.Favourites.FirstOrDefaultAsync(f => f.JourneyId == journeyId && f.UserId == userId, cancellationToken);

            if (favorite == null)
                return;

            _context.Favourites.Remove(favorite);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new FavouriteRemoved(favorite), cancellationToken);
        }

        public async Task<List<User>> GetUsersFavouritingJourneyAsync(int journeyId, CancellationToken cancellationToken)
        {
            return await _context.Favourites
                .Where(f => f.JourneyId == journeyId)
                .Select(f => f.User)
                .ToListAsync(cancellationToken);
        }
    }
}
