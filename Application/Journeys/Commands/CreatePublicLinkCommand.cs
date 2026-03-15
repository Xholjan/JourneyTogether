using MediatR;

namespace Application.Journeys.Commands
{
    public class CreatePublicLinkCommand : IRequest<string>
    {
        public int JourneyId { get; set; }
        public int UserId { get; set; }
    }
}
