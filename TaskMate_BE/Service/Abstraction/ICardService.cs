using TaskMate.DTOs.Card;

namespace TaskMate.Service.Abstraction;

public interface ICardService
{
    Task CreateAsync(CreateCardDto createCardDto);
    Task Remove(string AppUserId, Guid CardId);
    Task UpdateAsync(UpdateCardDto updateCardDto);
}
