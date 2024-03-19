namespace TaskMate.DTOs.Checkitem;

public class CreateCheckitemDto
{
    public string Text { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid ChecklistId { get; set; }
}
