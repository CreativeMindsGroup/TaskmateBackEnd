namespace TaskMate.DTOs.Checklist;

public class CreateChecklistDto
{
    public string Name { get; set; }
    public Guid CardId { get; set; }
    public Guid WorkspaceId { get; set; }
    public string AppUserId { get; set; }
}
