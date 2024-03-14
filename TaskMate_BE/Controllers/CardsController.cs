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


    [HttpPost]
    public async Task<IActionResult> CreateSlider([FromForm] CreateCardDto createCardDto)
    {
        await _cardService.CreateAsync(createCardDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSlider([FromForm] UpdateCardDto updateCardDto)
    {
        await _cardService.UpdateAsync(updateCardDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string AppUserId, Guid CardListId)
    {
        await _cardService.Remove(AppUserId, CardListId);
        return Ok();
    }
}
