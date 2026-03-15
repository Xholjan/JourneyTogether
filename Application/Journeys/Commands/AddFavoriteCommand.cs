using MediatR;

namespace Application.Journeys.Commands
{
    public class AddFavoriteCommand : IRequest
    {
        public int JourneyId { get; set; }
        public int UserId { get; set; }
    }
}
