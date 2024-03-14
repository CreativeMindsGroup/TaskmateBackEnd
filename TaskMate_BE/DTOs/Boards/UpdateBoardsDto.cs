namespace TaskMate.DTOs.Boards;

public class UpdateBoardsDto
{
    public Guid BoardId { get; set; }
    public string AppUserId { get; set; }
    public string Title { get; set; }
}
