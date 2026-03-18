using Domain.Entities;
using MediatR;

namespace Application.Users.Events
{
    public class UserStatusChanged : INotification
    {
        public User User { get; }
        public UserStatus OldStatus { get; }
        public UserStatus NewStatus { get; }

        public UserStatusChanged(User user, UserStatus oldStatus, UserStatus newStatus)
        {
            User = user;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
