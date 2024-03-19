using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class LabelCard:BaseEntity
{
    public Card Card { get; set; }
    public Guid CardId { get; set; }
    public Labels Label { get; set; }
    public Guid LabelId { get; set; }
}
