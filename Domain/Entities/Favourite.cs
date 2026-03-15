namespace Domain.Entities
{
    public class Favourite
    {
        public int Id { get; set; }
        public int JourneyId { get; set; }
        public Journey Journey { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
