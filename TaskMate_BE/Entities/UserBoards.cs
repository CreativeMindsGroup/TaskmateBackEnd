using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class UserBoards : BaseEntity
{
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public Boards Boards { get; set; }
    public Guid BoardsId { get; set; }
}