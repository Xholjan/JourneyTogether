using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class JourneyUpdated : INotification
    {
        public Journey Journey { get; set; }
        public Journey OldJourney { get; set; }

        public JourneyUpdated(Journey journey, Journey oldJourney)
        {
            Journey = journey;
            OldJourney = oldJourney;
        }
    }
}
