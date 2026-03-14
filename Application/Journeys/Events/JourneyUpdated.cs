using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class JourneyUpdated : INotification
    {
        public Journey Journey { get; }

        public JourneyUpdated(Journey journey)
        {
            Journey = journey;
        }
    }
}
