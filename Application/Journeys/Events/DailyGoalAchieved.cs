using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class DailyGoalAchieved : INotification
    {
        public Journey Journey { get; }

        public DailyGoalAchieved(Journey journey)
        {
            Journey = journey;
        }
    }
}
