using TaskMate.DTOs.Boards;
using TaskMate.DTOs.CardList;

namespace TaskMate.DTOs.Workspace;

public class GetWorkspaceInBoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public ICollection<GetBoardsDto> getBoardsDtos { get; set; } = new List<GetBoardsDto>();
}
