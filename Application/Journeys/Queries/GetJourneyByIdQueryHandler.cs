using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByIdQueryHandler : IRequestHandler<GetJourneyByIdQuery, Journey?>
    {
        private readonly IJourneyRepository _repo;
        public GetJourneyByIdQueryHandler(IJourneyRepository repo) => _repo = repo;

        public async Task<Journey?> Handle(GetJourneyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetJourneyByIdAsync(request.Id, cancellationToken);
        }
    }
}
