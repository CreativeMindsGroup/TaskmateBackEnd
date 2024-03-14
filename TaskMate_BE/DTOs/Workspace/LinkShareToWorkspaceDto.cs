namespace TaskMate.DTOs.Workspace;

public class LinkShareToWorkspaceDto
{
    public string AdminId { get; set; }
    public Guid WorkspaceId { get; set; }
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}
