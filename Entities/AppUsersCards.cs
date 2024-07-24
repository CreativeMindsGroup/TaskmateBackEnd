namespace TaskMate.Entities
{
    public class AppUsersCards
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid CardId { get; set; }
        public Card Card { get; set; }
    }
}
