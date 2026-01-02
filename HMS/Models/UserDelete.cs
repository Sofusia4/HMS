namespace HMS.Models
{
    public class UserDelete
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
