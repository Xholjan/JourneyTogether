using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Commands
{
    public class CreateJourneyCommand() : IRequest
    {
        public string StartLocation { get; set; } = string.Empty;
        public string ArrivalLocation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TransportType TransportType { get; set; }
        public decimal DistanceKm { get; set; }
        public int UserId { get; set; }
    }
}
