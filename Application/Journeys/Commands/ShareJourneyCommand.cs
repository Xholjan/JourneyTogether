using MediatR;

namespace Application.Journeys.Commands
{
    public class ShareJourneyCommand : IRequest
    {
        public int JourneyId { get; set; }
        public string? UserId { get; set; }
        public List<int> UserIds { get; set; } = new();

        public ShareJourneyCommand(int journeyId, string? userId, List<int> userIds)
        {
            JourneyId = journeyId;
            UserId = userId;
            UserIds = userIds;
        }
    }
}
