using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.Workspace;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
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
    [HttpGet("GetArchivedCards")]
    public async Task<List<GetArchivedCardDto>>  GetArchivedCards(Guid boardId)
    {
        var Cards = await _boardsService.GetArchivedCardsInBoard(boardId);
        return Cards;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateBoard(CreateBoardsDto createBoardsDto)
    {
        await _boardsService.CreateAsync(createBoardsDto);
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
        await _boardsService.Remove(dto.AppUserId, dto.BoardId,dto.WorkspaceId);
        return Ok();
    }
    [HttpPost("moveCard")]
    public async Task<IActionResult> MoveCard([FromBody] CardMoveDto moveDto)
    {
        try
        {
            await _boardsService.UpdateCardPositionAsync(moveDto.CardId, moveDto.SourceColumnId, moveDto.DestinationColumnId, moveDto.NewIndex);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost("moveCardList")]
    public async Task<IActionResult> UpdateCardListPositionAsync(Guid boardId,  List<Guid> newOrder)
    {
        try
        {
            await _boardsService.UpdateCardListPositionAsync(boardId, newOrder);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


}
