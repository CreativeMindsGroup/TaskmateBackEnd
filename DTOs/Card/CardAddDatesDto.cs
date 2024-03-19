namespace TaskMate.DTOs.Card;

public class CardAddDatesDto
{
    public Guid CardId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
