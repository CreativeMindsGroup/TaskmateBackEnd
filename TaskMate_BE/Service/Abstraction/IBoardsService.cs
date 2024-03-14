using TaskMate.DTOs.Boards;

namespace TaskMate.Service.Abstraction;

public interface IBoardsService
{
    Task CreateAsync(CreateBoardsDto createBoardsDto);
    Task AddUserBoard(AddUserBoardDto addUserBoard);
    Task ShareLinkBoardToUser(LinkShareToBoardDto linkShareToBoardDto);
    Task Remove(string AdminId, Guid BoardId);
    Task UpdateAsync(UpdateBoardsDto updateBoardsDto);
    Task<List<GetBoardsDto>> GetAllAsync(Guid WorkspaceId);
    Task<GetBoardsDto> GetByIdAsync(Guid BoardId);
}
