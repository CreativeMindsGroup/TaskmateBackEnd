namespace TaskMate.DTOs.Workspace;

public class UpdateWorkspaceDto
{
    public Guid WorkspaceId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string AppUserId { get; set; }
}