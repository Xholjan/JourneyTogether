using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneysPagedQueryHandler : IRequestHandler<GetJourneysPagedQuery, IEnumerable<Journey>>
    {
        private readonly IJourneyRepository _repo;

        public GetJourneysPagedQueryHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<IEnumerable<Journey>> Handle(GetJourneysPagedQuery request, CancellationToken cancellationToken)
        {
            var allJourneys = await _repo.GetJourneysAsync(cancellationToken);
            return allJourneys.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }
    }
}
