using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Card:BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    //Rellations
    public CardList CardList { get; set; }
    public Guid CardListId { get; set; }
    public List<Comment>? Comments { get; set; }
}
