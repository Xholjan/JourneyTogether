namespace Application.Journeys.Models
{
    public class JourneyModel
    {
        public int Id { get; set; }
        public string StartLocation { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public string ArrivalLocation { get; set; } = null!;
        public DateTime ArrivalTime { get; set; }
        public TransportType TransportType { get; set; }
        public decimal DistanceKm { get; set; }
        public bool IsFavourite { get; set; }
    }
}
