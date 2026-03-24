using Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetMonthlyDistanceQueryHandler : IRequestHandler<GetMonthlyDistanceQuery, PagedModel<MonthlyDistanceModel>>
    {
        private readonly IMonthlyDistanceRepository _repo;

        public GetMonthlyDistanceQueryHandler(IMonthlyDistanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedModel<MonthlyDistanceModel>> Handle(
            GetMonthlyDistanceQuery request,
            CancellationToken cancellationToken)
        {
            var query = await _repo.GetQueryableAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                query = request.OrderBy.ToLower() switch
                {
                    "distancekm" => request.Direction == "desc"
                        ? query.OrderByDescending(x => x.TotalDistanceKm)
                        : query.OrderBy(x => x.TotalDistanceKm),

                    "year" => request.Direction == "desc"
                        ? query.OrderByDescending(x => x.Year)
                        : query.OrderBy(x => x.Year),

                    "month" => request.Direction == "desc"
                        ? query.OrderByDescending(x => x.Month)
                        : query.OrderBy(x => x.Month),

                    _ => query
                };
            }

            var items = query
                .Select(x => new MonthlyDistanceModel
                {
                    UserId = x.UserId,
                    Year = x.Year,
                    Month = x.Month,
                    TotalDistanceKm = x.TotalDistanceKm
                });

            return new PagedModel<MonthlyDistanceModel>(items, request.Page, request.PageSize);
        }
    }
}
