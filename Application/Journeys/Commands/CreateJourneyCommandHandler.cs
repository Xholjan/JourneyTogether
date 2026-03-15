using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreateJourneyCommandHandler : IRequestHandler<CreateJourneyCommand>
    {
        private readonly IJourneyRepository _journeyRepo;
        private readonly IUserRepository _userRepo;

        public CreateJourneyCommandHandler(IJourneyRepository journeyRepo, IUserRepository userRepo)
        {
            _journeyRepo = journeyRepo;
            _userRepo = userRepo;
        }

        public async Task Handle(CreateJourneyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByAuth0Id(request.UserId, cancellationToken);

            var journey = new Journey
            {
                StartLocation = request.StartLocation,
                StartTime = request.StartTime,
                ArrivalLocation = request.ArrivalLocation,
                ArrivalTime = request.ArrivalTime,
                TransportType = (TransportType)request.TransportType,
                DistanceKm = request.DistanceKm,
                UserId = user.Id
            };

            await _journeyRepo.AddJourneyAsync(journey, cancellationToken);
        }
    }
}
