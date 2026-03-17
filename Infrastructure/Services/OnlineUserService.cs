using Application.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _onlineUsers = new();

        public void UserConnected(string userId, string connectionId)
        {
            var connections = _onlineUsers.GetOrAdd(userId, _ => new HashSet<string>());

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

        public void UserDisconnected(string userId, string connectionId)
        {
            if (_onlineUsers.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                        _onlineUsers.TryRemove(userId, out _);
                }
            }
        }

        public bool IsOnline(string userId)
        {
            return _onlineUsers.ContainsKey(userId);
        }
    }
}
