using TaskMate.DTOs.Workspace;

namespace TaskMate.Service.Abstraction;

public interface IWorkspaceService
{
    Task CreateAsync(CreateWorkspaceDto createWorkspaceDto);
    Task ShareLinkBoardToUser(LinkShareToWorkspaceDto linkShareToWorkspaceDto);
    Task AddUserWorkspace(AddUserWorkspace addUserWorkspace);
    Task UpdateAsync(UpdateWorkspaceDto updateWorkspaceDto);
    /// <summary>
    /// AdminId'si gelerse butun workspace'ler gorsenecekdir ama normal userId'si gelecekse yalniz hansi 
    /// Workspace'de varsa o gorseneckdir!!!
    /// </summary>
    /// <param name="AppUserId"></param>
    /// <returns></returns>
    Task<List<GetWorkspaceDto>> GetAllAsync(string AppUserId);

    Task<GetWorkspaceDto> GetByIdAsync(Guid WorspaceId);

    Task<List<GetWorkspaceInBoardDto>> GetWorkspaceInBoards(string AppUserId);
    Task Remove(string AppUserId, Guid WokspaceId);
}
