using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;

namespace TaskMate.Service.Abstraction;

public interface ICardService
{
    Task AddCardDateAsync(CardAddDatesDto cardAddDatesDto);
    Task CreateAsync(CreateCardDto createCardDto);
    Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto);
    Task<GetCardDto> GetByIdAsync(Guid cardId);
    Task Remove(string appUserId, Guid cardId);
    Task UpdateAsync(UpdateCardDto updateCardDto);
    Task<List<GetCardDto>> GetAllCardsByBoardIdAsync(Guid boardId);
    Task<List<GetCardDto>> GetAllCardsByCardListIdAsync(Guid cardListId);
    Task ReorderCardsAsync(ReorderCardsDto reorderCardsDto);
    Task ChangeCoverColorAsync(Guid cardId, string coverColor);
    Task UpdateCardDescriptionAsync(UpdateCardDescriptionDto updateCardDescriptionDto);
}
