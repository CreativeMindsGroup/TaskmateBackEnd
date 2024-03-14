using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Workspace;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardsController : ControllerBase
{
    private readonly IBoardsService _boardsService;
    public BoardsController(IBoardsService boardsService)
        => _boardsService = boardsService;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid WorkspaceId)
    {
        var boards = await _boardsService.GetAllAsync(WorkspaceId);
        return Ok(boards);
    }

    [HttpGet("{BoardId:Guid}")]
    public async Task<IActionResult> GetById(Guid BoardId)
    {
        var byWorkspace = await _boardsService.GetByIdAsync(BoardId);
        return Ok(byWorkspace);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlider([FromForm] CreateBoardsDto createBoardsDto)
    {
        await _boardsService.CreateAsync(createBoardsDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ShareLinkBoardUser([FromForm] LinkShareToBoardDto linkShareToBoardDto)
    {
        await _boardsService.ShareLinkBoardToUser(linkShareToBoardDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddBoard([FromForm] AddUserBoardDto addUserBoardDto)
    {
        await _boardsService.AddUserBoard(addUserBoardDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSlider([FromForm] UpdateBoardsDto updateBoardsDto)
    {
        await _boardsService.UpdateAsync(updateBoardsDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string AppUserId, Guid BoardId)
    {
        await _boardsService.Remove(AppUserId, BoardId);
        return Ok();
    }
}
