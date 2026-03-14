using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class JourneyDeleted : INotification
    {
        public Journey Journey { get; }

        public JourneyDeleted(Journey journey)
        {
            Journey = journey;
        }
    }
}
