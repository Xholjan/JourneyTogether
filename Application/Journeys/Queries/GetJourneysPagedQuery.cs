using Domain.Entities;
using MediatR;

namespace Application.Journeys.Queries
{
    public record GetJourneysPagedQuery(int UserId, int Page, int PageSize) : IRequest<IEnumerable<Journey>>;
}