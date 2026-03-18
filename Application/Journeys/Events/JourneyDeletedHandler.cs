using Application.Interfaces;
using Application.Notifications;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Journeys.Events
{
    public class JourneyDeletedHandler : INotificationHandler<JourneyDeleted>
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IOnlineUserService _onlineUserService;
        private readonly IEmailService _emailService;
        private readonly IMonthlyDistanceRepository _monthlyDistanceRepository;

        public JourneyDeletedHandler(
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

        public async Task Handle(JourneyDeleted notification, CancellationToken cancellationToken)
        {
            var journey = notification.Journey;

            await _monthlyDistanceRepository.RemoveAsync(
                journey.UserId,
                journey.StartTime,
                journey.DistanceKm,
                cancellationToken
            );
        }
    }
}