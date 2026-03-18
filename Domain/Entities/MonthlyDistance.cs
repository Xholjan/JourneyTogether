namespace Domain.Entities
{
    public class MonthlyDistance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalDistanceKm { get; set; }
    }
}
