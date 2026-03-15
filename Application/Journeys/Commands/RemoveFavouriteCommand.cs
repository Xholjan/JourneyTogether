using MediatR;

namespace Application.Journeys.Commands
{
    public class RemoveFavouriteCommand : IRequest
    {
        public int JourneyId { get; set; }
        public string? UserId { get; set; }

        public RemoveFavouriteCommand(int journeyId, string? userId)
        {
            JourneyId = journeyId;
            UserId = userId;
        }
    }
}
