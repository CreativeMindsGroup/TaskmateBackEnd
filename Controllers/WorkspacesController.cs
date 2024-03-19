using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Slider;
using TaskMate.DTOs.Workspace;
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

    [HttpGet("{WorkspaceId:Guid}")]
    public async Task<IActionResult> GetById(Guid WorkspaceId)
    {
        var byWorkspace = await _workspaceService.GetByIdAsync(WorkspaceId);
        return Ok(byWorkspace);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkspace(CreateWorkspaceDto createWorkspaceDto)
    {
        await _workspaceService.CreateAsync(createWorkspaceDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ShareLinkWorkspaceUser([FromForm] LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        await _workspaceService.ShareLinkBoardToUser(linkShareToWorkspaceDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddWorkspace([FromForm] AddUserWorkspace addUserWorkspace)
    {
        await _workspaceService.AddUserWorkspace(addUserWorkspace);
        return StatusCode((int)HttpStatusCode.Created);
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
        await _workspaceService.Remove(AppUserId,WorkspaceId);
        return Ok();
    }


}
