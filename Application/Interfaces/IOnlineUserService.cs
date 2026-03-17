namespace Application.Interfaces
{
    public interface IOnlineUserService
    {
        void UserConnected(string userId, string connectionId);
        void UserDisconnected(string userId, string connectionId);
        bool IsOnline(string userId);
    }
}
