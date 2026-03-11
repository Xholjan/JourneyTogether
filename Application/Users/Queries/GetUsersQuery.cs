using Domain.Entities;
using MediatR;

namespace Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<User>>;
}
