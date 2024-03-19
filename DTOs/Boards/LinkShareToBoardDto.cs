namespace TaskMate.DTOs.Boards;

public class LinkShareToBoardDto
{
    public string AdminId { get; set; }
    public Guid BoardId { get; set; }
    public Guid WorkspaceId { get; set; }
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}
