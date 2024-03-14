using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Workspace:BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    //Relation
    public List<Boards>? Boards { get; set; }
    public List<WorkspaceUser>? WorkspaceUsers { get; set; }
}
