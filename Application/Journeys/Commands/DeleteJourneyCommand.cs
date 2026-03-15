using MediatR;

namespace Application.Journeys.Commands
{
    public class DeleteJourneyCommand : IRequest
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        public DeleteJourneyCommand(int id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
