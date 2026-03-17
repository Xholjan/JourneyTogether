using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Journeys.Commands
{
    public class UpdateJourneyCommandHandler : IRequestHandler<UpdateJourneyCommand>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;

        public UpdateJourneyCommandHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(UpdateJourneyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var journey = await _journeyRepo.GetJourneyByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException("Journey not found");

            if (journey.UserId != user.Id)
                throw new CustomException("You can only update your own journeys");

            journey.StartLocation = request.StartLocation;
            journey.StartTime = request.StartTime;
            journey.ArrivalLocation = request.ArrivalLocation;
            journey.ArrivalTime = request.ArrivalTime;
            journey.TransportType = (Domain.Entities.TransportType)request.TransportType;
            journey.DistanceKm = request.DistanceKm;

            await _journeyRepo.UpdateJourneyAsync(journey, cancellationToken);
        }
    }
}
