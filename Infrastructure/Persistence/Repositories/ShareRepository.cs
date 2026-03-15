using Application.Interfaces.Application.Interfaces;
using Domain.Entities;
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
                        JourneyId = journeyId,
                        UserId = sharedByUserId,
                        Action = "Share",
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            public async Task<string> CreatePublicLinkAsync(int journeyId, int userId, CancellationToken cancellationToken)
            {
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
                    JourneyId = journeyId,
                    UserId = userId,
                    Action = "CreatePublicLink",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync(cancellationToken);

                return $"/public/journey/{token}";
            }
        }
    }
}
