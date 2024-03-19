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
    public async Task<IActionResult> GetAll(string AppUserId, Guid WorkspaceId)
    {
        var boards = await _boardsService.GetAllAsync(AppUserId, WorkspaceId);
        return Ok(boards);
    }


    [HttpGet("{BoardId:Guid}")]
    public async Task<IActionResult> GetById(Guid BoardId)
    {
        var byWorkspace = await _boardsService.GetByIdAsync(BoardId);
        return Ok(byWorkspace);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateBoard(CreateBoardsDto createBoardsDto)
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
    public async Task<IActionResult> UpdateBoard(UpdateBoardsDto updateBoardsDto)
    {
        await _boardsService.UpdateAsync(updateBoardsDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] BoardRemoveDto dto)
    {
        await _boardsService.Remove(dto.AppUserId, dto.BoardId);
        return Ok();
    }
}
