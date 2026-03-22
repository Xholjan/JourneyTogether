using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(int userId, User user, bool audit, CancellationToken cancellationToken);
        Task<User> GetByAuth0Id(string? auth0Id, CancellationToken cancellationToken);
        Task<User?> CheckAsync(string auth0Id, CancellationToken cancellationToken);
        Task<List<User>> GetUsersByIdsAsync(List<int> ids, CancellationToken cancellationToken);
    }
}
