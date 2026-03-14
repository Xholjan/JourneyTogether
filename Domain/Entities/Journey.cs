namespace Domain.Entities
{
    public class Journey
    {
        public int Id { get; set; }
        public string StartLocation { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public string ArrivalLocation { get; set; } = null!;
        public DateTime ArrivalTime { get; set; }
        public TransportType TransportType { get; set; }
        public decimal DistanceKm { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsDailyGoalAchieved { get; set; }
    }

    public enum TransportType
    {
        Car,
        Bus,
        Train,
        Plane,
        Bike,
        Walk
    }
}
