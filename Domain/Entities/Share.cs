namespace Domain.Entities
{
    public class Share
    {
        public int Id { get; set; }
        public int JourneyId { get; set; }
        public Journey Journey { get; set; } = null!;
        public int SharedWithUserId { get; set; }
        public User SharedWith { get; set; } = null!;
        public int SharedByUserId { get; set; }
        public User SharedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
