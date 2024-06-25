namespace TaskMate.DTOs.Card
{
    public class MakeArchiveDto
    {
        public bool isArchived { get; set; }
        public Guid CardId { get; set; }
        public Guid AdminId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
