namespace TaskMate.DTOs.Checkitem;

public class GetCheckitemDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime DueDate { get; set; }
    public bool Check { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModiffiedDate { get; set; }
    public virtual bool isDeleted { get; set; }
}
