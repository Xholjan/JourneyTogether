using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetMonthlyDistanceQuery : IRequest<PagedModel<MonthlyDistanceModel>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "UserId";
        public string Direction { get; set; } = "asc";
    }
}
