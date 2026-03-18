namespace Domain.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Action { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
