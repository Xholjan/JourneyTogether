using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class JourneyCreated : INotification
    {
        public Journey Journey { get; }

        public JourneyCreated(Journey journey)
        {
            Journey = journey;
        }
    }
}
