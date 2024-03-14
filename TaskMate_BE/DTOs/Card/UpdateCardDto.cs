namespace TaskMate.DTOs.Card;

public class UpdateCardDto
{
    public Guid CardId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CardListId { get; set; }
}
