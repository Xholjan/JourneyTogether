using Application.Exceptions;
using Application.Interfaces;
using Application.Journeys.Models;
using Application.Journeys.Queries;
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

        public async Task<List<JourneyModel>> GetAdminJourneysAsync(GetAdminJourneysQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Journeys.AsQueryable();

            if (request.Id.HasValue)
                query = query.Where(j => j.UserId == request.Id.Value);

            if (request.TransportType.HasValue)
                query = query.Where(j => j.TransportType == (Domain.Entities.TransportType)request.TransportType.Value);

            if (request.StartDateFrom.HasValue)
                query = query.Where(j => j.StartTime >= request.StartDateFrom.Value);

            if (request.StartDateTo.HasValue)
                query = query.Where(j => j.StartTime <= request.StartDateTo.Value);

            if (request.ArrivalDateFrom.HasValue)
                query = query.Where(j => j.ArrivalTime >= request.ArrivalDateFrom.Value);

            if (request.ArrivalDateTo.HasValue)
                query = query.Where(j => j.ArrivalTime <= request.ArrivalDateTo.Value);

            if (request.MinDistance.HasValue)
                query = query.Where(j => j.DistanceKm >= request.MinDistance.Value);

            if (request.MaxDistance.HasValue)
                query = query.Where(j => j.DistanceKm <= request.MaxDistance.Value);

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                query = (request.OrderBy.ToLower(), request.Direction?.ToLower()) switch
                {
                    ("starttime", "desc") => query.OrderByDescending(j => j.StartTime),
                    ("starttime", _) => query.OrderBy(j => j.StartTime),

                    ("arrivaltime", "desc") => query.OrderByDescending(j => j.ArrivalTime),
                    ("arrivaltime", _) => query.OrderBy(j => j.ArrivalTime),

                    ("distancekm", "desc") => query.OrderByDescending(j => j.DistanceKm),
                    ("distancekm", _) => query.OrderBy(j => j.DistanceKm),

                    _ => query
                };
            }

            var journeys = await query
                .Select(j => new JourneyModel
                {
                    Id = j.Id,
                    StartLocation = j.StartLocation,
                    ArrivalLocation = j.ArrivalLocation,
                    StartTime = j.StartTime,
                    ArrivalTime = j.ArrivalTime,
                    TransportType = (Application.Journeys.Models.TransportType)j.TransportType,
                    DistanceKm = j.DistanceKm,
                    IsFavourite = false
                })
                .ToListAsync(cancellationToken);

            return journeys;
        }

        public async Task AddJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            _context.Journeys.Add(journey);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JourneyCreated(journey), cancellationToken);
        }

        public async Task UpdateJourneyAsync(Journey journey, bool inCreate, CancellationToken cancellationToken)
        {
            var existing = await _context.Journeys
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == journey.Id, cancellationToken);

            if (existing == null)
                throw new CustomException("Journey not found");

            _context.Journeys.Update(journey);
            await _context.SaveChangesAsync(cancellationToken);

            if (!inCreate)
                await _mediator.Publish(new JourneyUpdated(journey, existing), cancellationToken);
        }

        public async Task DeleteJourneyAsync(Journey journey, CancellationToken cancellationToken)
        {
            var existing = await _context.Journeys
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == journey.Id, cancellationToken);

            if (existing == null)
                throw new CustomException("Journey not found");

            _context.Journeys.Remove(journey);
            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JourneyDeleted(existing), cancellationToken);
        }
    }
}
