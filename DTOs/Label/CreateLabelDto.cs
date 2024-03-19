namespace TaskMate.DTOs.Label;

public class CreateLabelDto
{
    public string? Name { get; set; }
    public string Color { get; set; }
    public Guid CardId { get; set; }
    public Guid BoardsId { get; set; }
}
