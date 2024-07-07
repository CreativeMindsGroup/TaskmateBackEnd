namespace TaskMate.DTOs.CustomFieldNumber;

public class CustomFieldNumberDto
{
    public string Title { get; set; }
    public string? Number { get; set; }
    public Guid CardId { get; set; }
    public string UserId { get; set; }
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
}
