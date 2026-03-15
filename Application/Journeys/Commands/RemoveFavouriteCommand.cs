using MediatR;

namespace Application.Journeys.Commands
{
    public class RemoveFavouriteCommand : IRequest
    {
        public int JourneyId { get; set; }
        public int UserId { get; set; }
    }
}
