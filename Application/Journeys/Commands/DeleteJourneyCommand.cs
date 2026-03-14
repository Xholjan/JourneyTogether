using MediatR;

namespace Application.Journeys.Commands
{
    public record DeleteJourneyCommand(int Id, int UserId) : IRequest<Unit>;
}
