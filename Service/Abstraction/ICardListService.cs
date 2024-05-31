using TaskMate.DTOs.CardList;

namespace TaskMate.Service.Abstraction;

public interface ICardListService
{
    Task CreateAsync(CreateCardListDto createCardListDto);
    Task<List<GetCardListDto>> GetAllCardListAsync(Guid BoardId);
    Task Remove(string AdminId, Guid CardlistId);
    Task UpdateAsync(UpdateeCardListDto updateeCardListDto);
    Task UpdateCardListOrdersAsync(UpdateCardListOrdersDto updateCardListOrdersDto);
}
