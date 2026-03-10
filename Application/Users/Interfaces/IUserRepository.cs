using Application.Users.Models;
using Domain.Entities;

namespace Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<User?> GetByAuth0Id(string auth0Id, CancellationToken cancellationToken);
    }
}
