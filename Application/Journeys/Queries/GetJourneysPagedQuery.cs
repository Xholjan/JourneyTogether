using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneysPagedQuery() : IRequest<IEnumerable<JourneyModel>>
    {
        public int UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}