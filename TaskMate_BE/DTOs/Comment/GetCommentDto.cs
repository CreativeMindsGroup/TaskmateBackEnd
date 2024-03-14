namespace TaskMate.DTOs.Comment;

public class GetCommentDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime CreateComment { get; set; }
    public string AppUserId { get; set; }
    public Guid CardId { get; set; }
}
