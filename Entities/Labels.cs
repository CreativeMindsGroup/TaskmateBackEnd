using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Labels:BaseEntity
{
    public string? Name { get; set; }
    public string Color { get; set; }
    public Boards Boards { get; set; }
    public Guid BoardsId { get; set; }
    public List<LabelCard>? LabelsCards { get; set; }
}
