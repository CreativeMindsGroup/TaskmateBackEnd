using TaskMate.DTOs.Boards;

namespace TaskMate.Service.Abstraction;

public interface IBoardsService
{
    Task CreateAsync(CreateBoardsDto createBoardsDto);
    Task Remove(string AdminId, Guid BoardId, Guid WorkspaceId);
    Task UpdateAsync(UpdateBoardsDto updateBoardsDto);
    Task<List<GetBoardsDto>> GetAllAsync(string AppUserId, Guid WorkspaceId);
    Task<GetBoardsDto> GetByIdAsync(Guid BoardId);
    Task UpdateCardPositionAsync(Guid cardId, Guid sourceColumnId, Guid destinationColumnId, int newPosition);
    Task UpdateCardListPositionAsync(Guid boardId, List<Guid> newOrder);
    Task<GetBoardsDto> GetArchivedTasks(Guid boardId);
}
