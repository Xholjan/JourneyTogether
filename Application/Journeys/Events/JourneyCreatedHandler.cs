using Application.Interfaces;
using Application.Notifications;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Journeys.Events
{
    public class JourneyCreatedHandler : INotificationHandler<JourneyCreated>
    {
        private readonly IJourneyRepository _repo;
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationHub> _hubContext;
        private const decimal DAILY_GOAL_KM = 10;

        public JourneyCreatedHandler(IJourneyRepository repo, IMediator mediator, IHubContext<NotificationHub> hubContext)
        {
            _repo = repo;
            _mediator = mediator;
            _hubContext = hubContext;
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

                await _hubContext.Clients.User(journey.UserId.ToString())
                    .SendAsync("DailyGoalAchieved", new { journey.Id, journey.UserId, totalDistanceToday });
            }
        }
    }
}
