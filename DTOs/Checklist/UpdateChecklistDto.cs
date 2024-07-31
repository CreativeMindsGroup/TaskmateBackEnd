namespace TaskMate.DTOs.Checklist;

public class UpdateChecklistDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid UserId { get; set; }
    public Guid WorkspaceId { get; set; }
}
