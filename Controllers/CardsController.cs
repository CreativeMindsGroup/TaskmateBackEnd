using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Card;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateCard(CreateCardDto createCardDto)
        {
            await _cardService.CreateAsync(createCardDto);
            return Ok();
        }
        [HttpGet]
        [Route("GetAllCardsByBoardId")]
        public async Task<IActionResult> GetAllCardsByBoardId(Guid boardId)
        {
            var cards = await _cardService.GetAllCardsByBoardIdAsync(boardId);
            return Ok(cards);
        }
        [HttpGet("GetAllCardsByCardListId")]
        public async Task<IActionResult> GetAllCardsByCardListId(Guid cardListId)
        {
            var cards = await _cardService.GetAllCardsByCardListIdAsync(cardListId);
            return Ok(cards);
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCard(UpdateCardDto updateCardDto)
        {
            await _cardService.UpdateAsync(updateCardDto);
            return Ok();
        }

        [HttpPost]
        [Route("dragAndDrop")]
        public async Task<IActionResult> DragAndDrop(DragAndDropCardDto dragAndDropCardDto)
        {
            await _cardService.DragAndDrop(dragAndDropCardDto);
            return Ok();
        }

        [HttpPost]
        [Route("addDates")]
        public async Task<IActionResult> AddCardDates(CardAddDatesDto cardAddDatesDto)
        {
            await _cardService.AddCardDateAsync(cardAddDatesDto);
            return Ok();
        }

        [HttpGet]
        [Route("{cardId}")]
        public async Task<IActionResult> GetCardById(Guid cardId)
        {
            var card = await _cardService.GetByIdAsync(cardId);
            return Ok(card);
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> RemoveCard(string appUserId, Guid cardId)
        {
            await _cardService.Remove(appUserId, cardId);
            return Ok();
        }
        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderCards([FromBody] ReorderCardsDto reorderCardsDto)
        {
            await _cardService.ReorderCardsAsync(reorderCardsDto);
            return Ok();
        }
        [HttpPut("update-description")]
        public async Task<IActionResult> UpdateDescription([FromBody] UpdateCardDescriptionDto updateCardDescriptionDto)
        {
            if (updateCardDescriptionDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                await _cardService.UpdateCardDescriptionAsync(updateCardDescriptionDto);
                return Ok("Description updated successfully.");
            }
            catch (NotFoundException)
            {
                return NotFound("Card not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("updateCover")]
        public async Task<IActionResult> ChangeCoverColorAsync(Guid cardId, string coverColor)
        {
            await _cardService.ChangeCoverColorAsync(cardId, coverColor);
            return Ok("Description updated successfully.");
        }
    }
}
