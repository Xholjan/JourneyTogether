using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications
{
    public class NotificationHub : Hub
    {
        public async Task SendDailyGoalAchieved(int userId, string message)
        {
            await Clients.User(userId.ToString()).SendAsync("DailyGoalAchieved", message);
        }
    }
}
