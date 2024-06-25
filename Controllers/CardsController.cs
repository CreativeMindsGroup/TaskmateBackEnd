using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Context;
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
        [HttpGet]

        [Route("GetAllArchived")]
        public async Task<List<GetCardDto>> getAllArchivedCards(Guid boardId)
        {
            var cards = await _cardService.getAllArchivedCards(boardId);
            return cards;
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
        [HttpPost("UploadAttacment")]
        public async Task<IActionResult> UploadFile(Guid CardId, string FileName, IFormFile file)
        {
            FileUploadDto dto = new();
            dto.FileName = FileName;
            dto.CardId = CardId;
            await _cardService.UploadAttachmentAsync(dto, file);
            return Ok(new { message = "File uploaded successfully" });
        }
        [HttpPut]
        [Route("AddCardDueDate")]
        public async Task<IActionResult> UpdateDueDate(CreateCardDueDateDto updateCheckitemDto)
        {
            await _cardService.UpdateDueDate(updateCheckitemDto);
            return Ok($"Added Due Date :{updateCheckitemDto.DueDate}");
        }
        [HttpPut]
        [Route("DueDateUpdated")]
        public async Task<IActionResult> DueDateDone(UpdateCardDueDateDto updateCheckitemDto)
        {
            await _cardService.DueDateDone(updateCheckitemDto);
            return Ok($" Due Date updated ");
        }

        [HttpPost]
        [Route("dragAndDrop")]
        public async Task<IActionResult> DragAndDrop(DragAndDropCardDto dragAndDropCardDto)
        {
            await _cardService.DragAndDrop(dragAndDropCardDto);
            return Ok();
        }  
        [HttpPut]
        [Route("UpdateTitle")]
        public async Task<IActionResult> UpdateTitle(UpdateTitleDto dto)
        {
            await _cardService.UpdateTitle(dto);
            return Ok("titleUpdated");
        }
        [HttpPost]
        [Route("IsArchived")]
        public async Task<IActionResult> MakeArchive(MakeArchiveDto dto)
        {
            await _cardService.MakeArchive(dto);
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
        [HttpGet]
        [Route("[Action]")]
        public async Task<List<CardAttachmentDto>> GetUploads(Guid CardId)
        {
            var card = await _cardService.GetUploads(CardId);
            return card;
        }
        [HttpGet("download/{cardId}/{fileName}")]
        public async Task<IActionResult> DownloadFile(Guid cardId, string fileName)
        {
            return await _cardService.DownloadFileAsync(cardId, fileName);
        }
        [HttpGet("{cardId}/attachments")]
        public async Task<MemoryStream> GetAttachments(Guid cardId)
        {
            return await _cardService.GetFiles(cardId);
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> RemoveCard(string appUserId, Guid cardId, Guid WorkspaceId)
        {
            await _cardService.Remove(appUserId, cardId, WorkspaceId);
            return Ok();
        }  
        [HttpDelete]
        [Route("RemoveFile")]
        public async Task<IActionResult> DeleteAttachment(Guid attachmentId, string userId, Guid workspaceId)
        {
            await _cardService.DeleteAttachment(attachmentId, userId, workspaceId);
            return Ok("Deleted!");
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
        [HttpPost("updateCover")]
        public async Task<IActionResult> ChangeCoverColorAsync(CardCoverCreateDto Dto)
        {
            await _cardService.ChangeCoverColorAsync(Dto);
            return Ok("Description updated successfully.");
        }

    }
}
