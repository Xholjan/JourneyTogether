using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByPublicIdQuery : IRequest<JourneyModel?>
    {
        public Guid Id { get; set; }
        public GetJourneyByPublicIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
