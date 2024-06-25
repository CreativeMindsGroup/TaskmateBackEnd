using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Auth;
using TaskMate.DTOs.Slider;
using TaskMate.DTOs.Users;
using TaskMate.DTOs.Workspace;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkspacesController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;
    public WorkspacesController(IWorkspaceService workspaceService)
        => _workspaceService = workspaceService;


    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllbyUserId(string AppUserId)
    {
        var workspaces = await _workspaceService.GetAllAsync(AppUserId);
        return Ok(workspaces);
    }


    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllInBoards(string AppUserId)
    {
        var workspaces = await _workspaceService.GetWorkspaceInBoards(AppUserId);
        return Ok(workspaces);
    }
    [HttpGet("[action]")]
    public async Task<List<GetUserDto>> GetAllUsersInWorkspace(Guid WorkspaceId, int page, int pageSize)
    {
        var workspaces = await _workspaceService.GetAllUsersInWorkspace(WorkspaceId, page, pageSize);
        return workspaces;
    }

    [HttpGet("{WorkspaceId:Guid}")]
    public async Task<IActionResult> GetById(Guid WorkspaceId)
    {
        var byWorkspace = await _workspaceService.GetByIdAsync(WorkspaceId);
        return Ok(byWorkspace);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateWorkspace(CreateWorkspaceDto createWorkspaceDto)
    {
        await _workspaceService.CreateAsync(createWorkspaceDto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpGet("[action]")]
    public async Task<RoleCountsDto> GetAllUsersInWorkspaceCount(Guid workspaceId)
    {
        var byWorkspace = await _workspaceService.GetAllUsersInWorkspaceCount(workspaceId);
        return byWorkspace;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSlider(UpdateWorkspaceDto updateWorkspaceDto)
    {
        await _workspaceService.UpdateAsync(updateWorkspaceDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string AppUserId, Guid WorkspaceId)
    {
        await _workspaceService.Remove(AppUserId, WorkspaceId);
        return Ok();
    }
    [HttpPost("generate-invite")]
    public async Task<IActionResult> GenerateWorkspaceInvite([FromBody] LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        try
        {
            var inviteLink = await _workspaceService.GenerateTokenForWorkspaceInvite(linkShareToWorkspaceDto);
            return Ok(new { InviteLink = inviteLink });
        }
        catch (System.Exception ex)
        {
            return BadRequest($"Error generating invite link: {ex.Message}");
        }
    }

    // Accepts an invitation to a workspace using a token
    [HttpPost("accept-invite")]
    public async Task<IActionResult> AcceptWorkspaceInvite([FromQuery] string token, string UserEmail)
    {
        try
        {
            return await _workspaceService.InviteUserToWorkspace(token, UserEmail);
        }
        catch (System.Exception ex)
        {
            return BadRequest($"Error accepting invite: {ex.Message}");
        }
    }
    [HttpPost("AddUserToWorkspace")]
    public async Task<IActionResult> AddUserToWorkspace([FromBody] LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        try
        {
            await _workspaceService.AddNewUserToWorkspace(linkShareToWorkspaceDto);
            return Ok("user Added!");
        }
        catch (System.Exception ex)
        {
            return BadRequest($"Error accepting invite: {ex.Message}");
        }
    }
    [HttpPost("changeUserRole")]
    public async Task<IActionResult> ChangeUserRole([FromBody] UpdateUserRoleDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _workspaceService.ChangeUserRole(dto);
        return Ok();
    }   
    [HttpDelete("RemoveUserFromWorkspace")]
    public async Task<IActionResult> RemoveUserFromWorkspace([FromBody] RemoveUserFromWorksapceDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _workspaceService.RemoveUserFromWorkspace(dto);
        return Ok();
    }

}
