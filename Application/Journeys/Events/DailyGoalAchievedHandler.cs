using Application.Interfaces;
using Application.Notifications;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Journeys.Events
{
    public class DailyGoalAchievedHandler : INotificationHandler<DailyGoalAchieved>
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserRepository _userRepo;

        public DailyGoalAchievedHandler(IHubContext<NotificationHub> hubContext, IUserRepository userRepo)
        {
            _hubContext = hubContext;
            _userRepo = userRepo;
        }

        public async Task Handle(DailyGoalAchieved notification, CancellationToken cancellationToken)
        {
            var journey = notification.Journey;
            var user = await _userRepo.GetByIdAsync(journey.UserId, cancellationToken);

            await _hubContext.Clients.User(user?.Auth0Id)
                .SendAsync("DailyGoalAchieved", new
                {
                    journey.Id,
                    journey.UserId,
                    journey.DistanceKm
                });
        }
    }
}
