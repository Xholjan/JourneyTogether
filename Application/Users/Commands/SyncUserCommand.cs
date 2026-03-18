using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands
{
    public class SyncUserCommand : IRequest<UserStatus>
    {
        public string? Auth0Id { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Role { get; }
        public SyncUserCommand(string? auth0Id, string email, string firstName, string lastName, string role)
        {
            Auth0Id = auth0Id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }
    }
}
