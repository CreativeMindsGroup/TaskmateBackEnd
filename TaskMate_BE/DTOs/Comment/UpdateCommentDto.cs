namespace TaskMate.DTOs.Comment;

public class UpdateCommentDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string AppUserId { get; set; }
    public Guid CardId { get; set; }
}
