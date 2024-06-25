using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardListsController : ControllerBase
    {
        private readonly ICardListService _cardListService;

        public CardListsController(ICardListService cardListService)
        {
            _cardListService = cardListService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateCardList(CreateCardListDto createCardListDto)
        {
            await _cardListService.CreateAsync(createCardListDto);
            return Ok("card list Created!");
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateCardList(UpdateTitleDto updateeCardListDto)
        {
            await _cardListService.UpdateAsync(updateeCardListDto);
            return Ok();
        }
        [HttpPost("updateOrders")]
        public async Task<IActionResult> UpdateCardListOrders([FromBody] UpdateCardListOrdersDto updateCardListOrdersDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _cardListService.UpdateCardListOrdersAsync(updateCardListOrdersDto);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllCardListbyBoardId")]
        public async Task<IActionResult> GetAllCardLists(Guid boardId)
        {
            var cardLists = await _cardListService.GetAllCardListAsync(boardId);
            return Ok(cardLists);
        }
        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> RemoveCardList(Guid CardlistId, Guid WorkspaceId, string UserId)
        {
            await _cardListService.Remove(CardlistId, WorkspaceId, UserId);
            return Ok();
        }
    }
}
