namespace TaskMate.DTOs.Boards
{
    public class BoardRemoveDto
    {
        public string AppUserId { get; set; }
        public Guid BoardId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
