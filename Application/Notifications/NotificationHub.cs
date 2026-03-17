using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications
{
    public class NotificationHub : Hub
    {
        private readonly IOnlineUserService _onlineUserService;

        public NotificationHub(IOnlineUserService onlineUserService)
        {
            _onlineUserService = onlineUserService;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            _onlineUserService.UserConnected(userId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            _onlineUserService.UserDisconnected(userId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
