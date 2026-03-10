using Application.Users.Models;
using MediatR;

namespace Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<List<UserModel>>;
}
