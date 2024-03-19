namespace TaskMate.DTOs.Boards;

public class AddUserBoardDto
{
    public string AdminId { get; set; }
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public string AppUserId { get; set; }
}
