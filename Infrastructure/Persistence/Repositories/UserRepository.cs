using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(int userId, User user, bool audit, CancellationToken cancellationToken)
        {
            if (audit)
            {
                _context.Audits.Add(new Audit
                {
                    UserId = userId,
                    Action = "Updated user with Id = " + user.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetByAuth0Id(string? auth0Id, CancellationToken cancellationToken)
        {
            if (auth0Id == null)
                throw new CustomException("User not found");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Auth0Id == auth0Id, cancellationToken);

            if (user == null)
                throw new CustomException("User not found");

            return user;
        }

        public async Task<User?> CheckAsync(string auth0Id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Auth0Id == auth0Id, cancellationToken);

            return user;
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
