using Application.Users.Interfaces;
using Application.Users.Models;
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

        public async Task<List<UserModel>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<User?> GetByAuth0Id(string auth0Id, CancellationToken ct)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Auth0Id == auth0Id, ct);
        }
    }
}
