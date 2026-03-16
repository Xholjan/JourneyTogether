using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Journeys.Events
{
    public class JourneyCreatedHandler : INotificationHandler<JourneyCreated>
    {
        private readonly IJourneyRepository _repo;
        private readonly IMediator _mediator;
        private const decimal DAILY_GOAL_KM = 10;

        public JourneyCreatedHandler(IJourneyRepository repo, IMediator mediator)
        {
            _repo = repo;
            _mediator = mediator;
        }

        public async Task Handle(JourneyCreated notification, CancellationToken cancellationToken)
        {
            var journey = notification.Journey;

            var todayJourneys = (await _repo.GetJourneysAsync(journey.UserId, cancellationToken))
                .Where(j => j.StartTime.Date == DateTime.UtcNow.Date)
                .ToList();

            var alreadyAchieved = todayJourneys.Any(j => j.IsDailyGoalAchieved);
            if (alreadyAchieved)
                return;

            var totalDistanceToday = todayJourneys.Sum(j => j.DistanceKm);

            if (totalDistanceToday >= DAILY_GOAL_KM)
            {
                journey.IsDailyGoalAchieved = true;
                await _repo.UpdateJourneyAsync(journey, cancellationToken);

                await _mediator.Publish(new DailyGoalAchieved(journey), cancellationToken);
            }
        }
    }
}
