using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands
{
    public class UpdateUserStatusCommand : IRequest
    {
        public string? UserId { get; set; }
        public int Id { get; set; }
        public UserStatus NewStatus { get; set; }
    }
}
