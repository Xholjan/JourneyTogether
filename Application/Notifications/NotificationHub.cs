using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
