using Application.Interfaces;
using Application.Notifications;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Journeys.Events
{
    public class JourneyUpdatedEventHandler : INotificationHandler<JourneyUpdated>
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IOnlineUserService _onlineUserService;
        private readonly IEmailService _emailService;
        private readonly IMonthlyDistanceRepository _monthlyDistanceRepository;

        public JourneyUpdatedEventHandler(
            IFavouriteRepository favouriteRepository,
            IUserRepository userRepository,
            IHubContext<NotificationHub> hubContext,
            IOnlineUserService onlineUserService,
            IEmailService emailService,
            IMonthlyDistanceRepository monthlyDistanceRepository)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
            _onlineUserService = onlineUserService;
            _emailService = emailService;
            _monthlyDistanceRepository = monthlyDistanceRepository;
        }

        public async Task Handle(JourneyUpdated notification, CancellationToken cancellationToken)
        {
            var journey = notification.Journey;
            var old = notification.OldJourney;

            var sameBucket =
                old.UserId == journey.UserId &&
                old.StartTime.Year == journey.StartTime.Year &&
                old.StartTime.Month == journey.StartTime.Month;

            if (sameBucket)
            {
                var diff = journey.DistanceKm - old.DistanceKm;

                if (diff != 0)
                {
                    await _monthlyDistanceRepository.AddOrUpdateAsync(
                        journey.UserId,
                        journey.StartTime,
                        diff,
                        cancellationToken
                    );
                }
            }
            else
            {
                await _monthlyDistanceRepository.MoveAsync(
                    old.UserId, old.StartTime, old.DistanceKm,
                    journey.UserId, journey.StartTime, journey.DistanceKm,
                    cancellationToken
                );
            }

            var userIds = await _favouriteRepository
                .GetUserIdsByJourneyIdAsync(journey.Id, cancellationToken);

            if (!userIds.Any())
                return;

            var users = await _userRepository
                .GetUsersByIdsAsync(userIds, cancellationToken);

            foreach (var user in users)
            {
                if (_onlineUserService.IsOnline(user.Auth0Id))
                {
                    await _hubContext.Clients.User(user.Auth0Id)
                        .SendAsync("JourneyUpdated", new
                        {
                            JourneyId = journey.Id,
                            Message = $"Journey {journey.StartLocation} → {journey.ArrivalLocation} was updated"
                        }, cancellationToken);
                }
                else
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Journey Updated",
                        $"Journey {journey.StartLocation} → {journey.ArrivalLocation} was updated."
                    );
                }
            }
        }
    }
}