namespace TaskMate.DTOs.Comment;

public class CreateCommentDto
{
    public string Message { get; set; }
    public string AppUserId { get; set; }
    public Guid CardId { get; set; }

}
