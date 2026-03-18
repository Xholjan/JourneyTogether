using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetAdminJourneysQuery : IRequest<PagedModel<JourneyModel>>
    {
        public string? UserId { get; set; }
        public int? Id { get; set; }
        public TransportType? TransportType { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? ArrivalDateFrom { get; set; }
        public DateTime? ArrivalDateTo { get; set; }
        public decimal? MinDistance { get; set; }
        public decimal? MaxDistance { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "StartTime";
        public string Direction { get; set; } = "asc";
    }
}
