using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Application.Notifications
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? connection.User.FindFirst("sub")?.Value;
        }
    }
}
