using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class WorkspaceUser:BaseEntity
{
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public Workspace Workspace { get; set; }
    public Guid WorkspaceId { get; set; }
}
