namespace TaskMate.DTOs.Workspace;

public class CreateWorkspaceDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string AppUserId { get; set; }
}
