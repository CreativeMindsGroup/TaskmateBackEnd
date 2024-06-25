using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Auth;
using TaskMate.DTOs.Users;
using TaskMate.DTOs.Workspace;

namespace TaskMate.Service.Abstraction;

public interface IWorkspaceService
{
    Task CreateAsync(CreateWorkspaceDto createWorkspaceDto);
    Task UpdateAsync(UpdateWorkspaceDto updateWorkspaceDto);
    Task<List<GetWorkspaceDto>> GetAllAsync(string AppUserId);
    Task<GetWorkspaceDto> GetByIdAsync(Guid WorspaceId);
    Task<List<GetUserDto>> GetAllUsersInWorkspace(Guid WorkspaceId, int page, int pageSize);
    Task<List<GetWorkspaceInBoardDto>> GetWorkspaceInBoards(string AppUserId);
    Task Remove(string AppUserId, Guid WokspaceId);
    Task<RoleCountsDto> GetAllUsersInWorkspaceCount(Guid workspaceId);
    Task<string> GenerateTokenForWorkspaceInvite(LinkShareToWorkspaceDto linkShareToWorkspaceDto);
    Task<IActionResult> InviteUserToWorkspace(string token, string UserEmail);
    Task AddNewUserToWorkspace(LinkShareToWorkspaceDto linkShareToWorkspaceDto);
    Task ChangeUserRole(UpdateUserRoleDto dto);
    Task RemoveUserFromWorkspace(RemoveUserFromWorksapceDto dto);
}
