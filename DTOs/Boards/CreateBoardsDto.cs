namespace TaskMate.DTOs.Boards;

public class CreateBoardsDto
{
    public string AppUserId { get; set; }
    public string Title { get; set; }
    public string Theme { get; set; }
    public Guid WorkspaceId { get; set; }
}
