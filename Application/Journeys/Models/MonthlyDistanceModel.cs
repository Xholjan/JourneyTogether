namespace Application.Journeys.Models
{
    public class MonthlyDistanceModel
    {
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalDistanceKm { get; set; }
    }
}
