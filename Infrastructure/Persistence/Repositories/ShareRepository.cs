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

                await _context.SaveChangesAsync(cancellationToken);
            }

            public async Task<Guid> CreatePublicLinkAsync(int journeyId, int userId, CancellationToken cancellationToken)
            {
                var existingLink = await _context.PublicLinks.Where(p => p.JourneyId == journeyId && !p.IsRevoked)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingLink != null)
                    return existingLink.Token;

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

                return link?.Journey;
            }
        }
    }
}
