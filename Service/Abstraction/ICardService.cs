using TaskMate.DTOs.Card;

namespace TaskMate.Service.Abstraction;

public interface ICardService
{
    Task CreateAsync(CreateCardDto createCardDto);
    Task<GetCardDto> GetByIdAsync(Guid CardId);
    Task Remove(string AppUserId, Guid CardId);
    Task UpdateAsync(UpdateCardDto updateCardDto);
    Task AddCardDateAsync(CardAddDatesDto cardAddDatesDto);
    Task MoveCardAsync(MoveCard moveCard);
    Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto);
}
