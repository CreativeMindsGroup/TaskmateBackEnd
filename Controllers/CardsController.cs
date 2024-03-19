using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;
    public CardsController(ICardService cardService)
        => _cardService = cardService;


    [HttpGet("{CarddId:Guid}")]
    public async Task<IActionResult> GetById(Guid CarddId)
    {
        var byWorkspace = await _cardService.GetByIdAsync(CarddId);
        return Ok(byWorkspace);
    }


    [HttpPost]
    public async Task<IActionResult> CreateCard([FromForm] CreateCardDto createCardDto)
    {
        await _cardService.CreateAsync(createCardDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCard([FromForm] UpdateCardDto updateCardDto)
    {
        await _cardService.UpdateAsync(updateCardDto);
        return Ok();
    }   
    
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCard([FromForm] CardAddDatesDto cardAddDatesDto)
    {
        await _cardService.AddCardDateAsync(cardAddDatesDto);
        return Ok();
    }  
    
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCardDragAndDrop([FromForm] DragAndDropCardDto dragAndDropCardDto)
    {
        await _cardService.DragAndDrop(dragAndDropCardDto);
        return Ok();
    }
      
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCardMoveCard([FromForm] MoveCard moveCard)
    {
        await _cardService.MoveCardAsync(moveCard);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string AppUserId, Guid CardId)
    {
        await _cardService.Remove(AppUserId, CardId);
        return Ok();
    }
}
