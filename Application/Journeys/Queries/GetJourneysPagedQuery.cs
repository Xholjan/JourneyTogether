using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneysPagedQuery : IRequest<PagedModel<JourneyModel>>
    {
        public string? UserId { get; set; } = string.Empty;
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetJourneysPagedQuery(string? userId, int page, int pageSize)
        {
            UserId = userId;
            Page = page;
            PageSize = pageSize;
        }
    }
}