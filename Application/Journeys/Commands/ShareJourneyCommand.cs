using MediatR;

namespace Application.Journeys.Commands
{
    public class ShareJourneyCommand : IRequest
    {
        public int JourneyId { get; set; }
        public int SharedByUserId { get; set; }
        public List<int> UserIds { get; set; } = new();
    }
}
