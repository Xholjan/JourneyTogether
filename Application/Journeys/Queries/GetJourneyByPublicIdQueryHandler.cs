using Application.Interfaces.Application.Interfaces;
using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    internal class GetJourneyByPublicIdQueryHandler : IRequestHandler<GetJourneyByPublicIdQuery, JourneyModel?>
    {
        private readonly IShareRepository _shareRepo;
        public GetJourneyByPublicIdQueryHandler(IShareRepository shareRepo)
        {
            _shareRepo = shareRepo;
        }

        public async Task<JourneyModel?> Handle(GetJourneyByPublicIdQuery request, CancellationToken cancellationToken)
        {
            var share = await _shareRepo.GetSharedJourneyByGuidAsync(request.Id, cancellationToken);

            return share is null ? null : new JourneyModel
            {
                Id = share.Id,
                StartLocation = share.StartLocation,
                StartTime = share.StartTime,
                ArrivalLocation = share.ArrivalLocation,
                ArrivalTime = share.ArrivalTime,
                TransportType = (TransportType)share.TransportType,
                DistanceKm = share.DistanceKm
            };
        }
    }
}
