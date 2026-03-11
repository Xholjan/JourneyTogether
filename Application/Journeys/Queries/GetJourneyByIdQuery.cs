using Domain.Entities;
using MediatR;

namespace Application.Journeys.Queries
{
    public record GetJourneyByIdQuery(int Id) : IRequest<Journey?>;
}
