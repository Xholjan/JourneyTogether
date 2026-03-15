using MediatR;

namespace Application.Journeys.Commands
{
    public class AddFavoriteCommand : IRequest
    {
        public int JourneyId { get; set; }
        public string? UserId { get; set; }

        public AddFavoriteCommand(int journeyId, string? userId)
        {
            JourneyId = journeyId;
            UserId = userId;
        }
    }
}
