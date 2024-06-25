namespace TaskMate.DTOs.Boards;

public class UpdateBoardsDto
{
    public Guid BoardId { get; set; }
    public Guid WorkspaceId { get; set; }
    public string AppUserId { get; set; }
    public string Title { get; set; }
    public string Theme { get; set; }
}

public class MoveCardRequest
{
    public Guid ColumnId { get; set; }
    public List<CardPosition> CardOrders { get; set; }
}

public class CardPosition
{
    public Guid CardId { get; set; }  
    public int NewPosition { get; set; }
}
public class UpdateCardListOrderDto
{
    public Guid boardId { get; set; }
    public List<Guid> NewOrder { get; set; }
}
