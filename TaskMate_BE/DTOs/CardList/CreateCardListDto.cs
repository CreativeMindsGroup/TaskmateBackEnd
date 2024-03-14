namespace TaskMate.DTOs.CardList;

public class CreateCardListDto
{
    public string AppUserId { get; set; }
    public string Title { get; set; }
    public Guid BoardsId { get; set; }

}
