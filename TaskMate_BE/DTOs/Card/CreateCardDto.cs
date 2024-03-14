namespace TaskMate.DTOs.Card;

public class CreateCardDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CardListId { get; set; }
}
