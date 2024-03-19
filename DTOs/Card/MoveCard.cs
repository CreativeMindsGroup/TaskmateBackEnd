namespace TaskMate.DTOs.Card;

public class MoveCard
{
    public string AppUserId { get; set; }
    public Guid BoardId { get; set; }
    public Guid CardListId { get; set; }
    public Guid CardId { get; set; }
}
