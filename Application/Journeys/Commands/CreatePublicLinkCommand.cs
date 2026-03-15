using MediatR;

namespace Application.Journeys.Commands
{
    public class CreatePublicLinkCommand : IRequest<string>
    {
        public int JourneyId { get; set; }
        public string? UserId { get; set; }

        public CreatePublicLinkCommand(int journeyId, string? userid)
        {
            JourneyId = journeyId;
            UserId = userid;
        }
    }
}
