using TaskMate.Helper.Enum.User;

namespace TaskMate.DTOs.Workspace;

public class LinkShareToWorkspaceDto
{
    public string AdminId { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public Guid WorkspaceId { get; set; }
}
