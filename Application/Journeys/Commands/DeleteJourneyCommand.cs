using MediatR;

namespace Application.Journeys.Commands
{
    public record DeleteJourneyCommand(int Id, Guid UserId) : IRequest<Unit>;
}
