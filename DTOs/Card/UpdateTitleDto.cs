namespace TaskMate.DTOs.Card
{
    public class UpdateTitleDto
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
