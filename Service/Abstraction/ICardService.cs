using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;
using TaskMate.DTOs.CardMembers;
using TaskMate.Service.Implementations;

namespace TaskMate.Service.Abstraction;

public interface ICardService
{
    Task AddCardDateAsync(CardAddDatesDto cardAddDatesDto);
    Task CreateAsync(CreateCardDto createCardDto);
    Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto);
    Task<GetCardDto> GetByIdAsync(Guid cardId);
    Task Remove(string appUserId, Guid cardId, Guid WorkspaceId);
    Task UpdateAsync(UpdateCardDto updateCardDto);
    Task<List<GetCardDto>> GetAllCardsByBoardIdAsync(Guid boardId);
    Task<List<GetCardDto>> GetAllCardsByCardListIdAsync(Guid cardListId);
    Task ReorderCardsAsync(ReorderCardsDto reorderCardsDto);
    Task ChangeCoverColorAsync(CardCoverCreateDto Dto);
    Task UpdateCardDescriptionAsync(UpdateCardDescriptionDto updateCardDescriptionDto);
    Task UpdateDueDate(CreateCardDueDateDto updateCheckitemDto);
    Task DueDateDone(UpdateCardDueDateDto Dto);
    Task UploadAttachmentAsync(FileUploadDto uploadDto, IFormFile UploadFile);
    Task<IActionResult> DownloadFileAsync(Guid cardId, string fileName);
    Task<List<CardAttachmentDto>> GetUploads(Guid CardId);
    Task MakeArchive(MakeArchiveDto dto);
    Task<MemoryStream> GetFiles(Guid cardId);
    Task DeleteAttachment(Guid attachmentId, string userId, Guid workspaceId);
    Task UpdateTitle(UpdateTitleDto dto);
    Task<List<GetCardDto>> getAllArchivedCards(Guid boardId);
    Task<bool> AddUserToCard(AddMemberToCardDto dto);
    Task<bool> RemoveUserFromCard(AddMemberToCardDto dto);
}
