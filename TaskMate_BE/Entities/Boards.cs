using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class Boards : BaseEntity
{
    public string Title { get; set; }

    //Rellations
    public List<CardList>? CardLists { get; set; }
    public Workspace Workspace { get; set; }
    public Guid WorkspaceId { get; set; }
    public List<UserBoards>? UserBoards { get; set; }

}