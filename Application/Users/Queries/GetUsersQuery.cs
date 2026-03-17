using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserModel>>
    {
        public string? UserId { get; set; }
        public GetUsersQuery(string? userId)
        {
            UserId = userId;
        }
    }
}
