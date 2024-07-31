using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Checkitem;
using TaskMate.DTOs.Checklist;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChecklistsController : ControllerBase
{
    private readonly IChecklistService _checklistService;
    public ChecklistsController(IChecklistService checklistService)
        => _checklistService = checklistService;


    [HttpGet]
    public async Task<IActionResult> GetAll(Guid CardId)
    {
        var boards = await _checklistService.GetAllAsync(CardId);
        return Ok(boards);
    }

    [HttpPost]
    public async Task<IActionResult> Createchecklist([FromForm] CreateChecklistDto createChecklistDto)
    {
        await _checklistService.CreateAsync(createChecklistDto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpPut("ChangeChecklistTitle")]
    public async Task<IActionResult> UpdateCheckItemTitle(UpdateChecklistDto updateChecklistDto)
    {
        await _checklistService.EditChecklistTitle(updateChecklistDto);
        return Ok("Updated!");
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid CheckListId,Guid WorkspaceId , string UserId)
    {
        await _checklistService.RemoveAsync(CheckListId, WorkspaceId, UserId );
        return Ok("Removed");
    }
}
