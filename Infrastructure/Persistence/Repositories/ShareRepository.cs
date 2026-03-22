using Application.Exceptions;
using Application.Interfaces.Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Persistence.Repositories
{
    namespace Persistence.Repositories
    {
        public class ShareRepository : IShareRepository
        {
            private readonly ApplicationDbContext _context;

            public ShareRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task ShareJourneyAsync(int journeyId, int sharedByUserId, List<int> userIds, CancellationToken cancellationToken)
            {
                foreach (var userId in userIds)
                {
                    var check = _context.Shares.AsNoTracking().Where(s => s.JourneyId == journeyId && s.SharedByUserId == sharedByUserId && s.SharedWithUserId == userId);
                    if (check == null)
                    {
                        var share = new Share
                        {
                            JourneyId = journeyId,
                            SharedByUserId = sharedByUserId,
                            SharedWithUserId = userId,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.Shares.Add(share);

                        _context.Audits.Add(new Audit
                        {
                            UserId = sharedByUserId,
                            Action = "Shared journey with Id = " + journeyId,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            public async Task<Guid> CreatePublicLinkAsync(int journeyId, int userId, CancellationToken cancellationToken)
            {
                var existingLink = await _context.PublicLinks.Where(p => p.JourneyId == journeyId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingLink != null)
                {
                    if (existingLink.IsRevoked)
                    {
                        existingLink.IsRevoked = false;
                        existingLink.CreatedAt = DateTime.UtcNow;
                        _context.Audits.Add(new Audit
                        {
                            UserId = userId,
                            Action = "Reactivated public link for journey with Id = " + journeyId,
                            CreatedAt = DateTime.UtcNow
                        });

                        _context.PublicLinks.Update(existingLink);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    return existingLink.Token;
                }

                var token = Guid.NewGuid();

                var link = new PublicLink
                {
                    JourneyId = journeyId,
                    Token = token,
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PublicLinks.Add(link);

                _context.Audits.Add(new Audit
                {
                    UserId = userId,
                    Action = "Created public link for journey with Id = " + journeyId,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync(cancellationToken);
                return token;
            }

            public async Task<Journey?> GetSharedJourneyByGuidAsync(Guid token, CancellationToken cancellationToken)
            {
                var link = await _context.PublicLinks
                    .Include(p => p.Journey)
                    .Where(p => p.Token == token && !p.IsRevoked)
                    .FirstOrDefaultAsync(cancellationToken);

                if (link != null)
                {
                    link.IsRevoked = true;
                    _context.Audits.Add(new Audit
                    {
                        UserId = 1,
                        Action = "Revoked public link for journey with Id = " + link.JourneyId,
                        CreatedAt = DateTime.UtcNow
                    });

                    _context.PublicLinks.Update(link);
                    await _context.SaveChangesAsync(cancellationToken);

                    return link?.Journey;
                }
                else
                {
                    throw new GoneException("Invalid or expired link");
                }
            }

            public async Task<Share?> GetSharedJourneyAsync(int journeyId, int userId, CancellationToken cancellationToken)
            {
                return await _context.Shares
                    .Include(s => s.Journey)
                    .Where(s => s.JourneyId == journeyId && s.SharedWithUserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            public async Task DeleteSharedJourneyAsync(int id, int userId, CancellationToken cancellationToken)
            {
                var existing = await _context.Shares.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

                if (existing == null)
                    throw new CustomException("Journey not found");

                _context.Audits.Add(new Audit
                {
                    UserId = userId,
                    Action = "Unshared journey with Id = " + existing.JourneyId,
                    CreatedAt = DateTime.UtcNow
                });


                _context.Shares.Remove(existing);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
