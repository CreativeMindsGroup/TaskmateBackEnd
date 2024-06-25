namespace TaskMate.DTOs.Card
{
    public class CardCoverCreateDto
    {
        public Guid CardId { get; set; }
        public Guid AdminId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Color { get; set; }
    }
}
 