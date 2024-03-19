using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Checklist:BaseEntity
{
    public string Name { get; set; }
    public int CheckPercentage { get; set; } = 0;
    //Rellations
    public Card Card { get; set; }
    public Guid CardId { get; set; }
    public List<Checkitem>? Checkitems { get; set; }
}
