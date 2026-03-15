using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByIdQuery() : IRequest<JourneyModel?>
    {
        public int Id { get; set; }
    }
}
