using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CardList:BaseEntity
{
    public string Title { get; set; }
    public int Order {  get; set; } 
    //Rellatios
    public  List<Card>? Cards { get; set; }
    public Boards Boards { get; set; }
    public Guid BoardsId { get; set; }
}
