using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;

namespace TaskMate.Service.Abstraction;

public interface ICardListService
{
    Task CreateAsync(CreateCardListDto createCardListDto);
    Task<List<GetCardListDto>> GetAllCardListAsync(Guid BoardId);
    Task Remove(Guid CardlistId, Guid WorkspaceId, string UserId);
    Task UpdateAsync(UpdateTitleDto dto);
    Task UpdateCardListOrdersAsync(UpdateCardListOrdersDto updateCardListOrdersDto);
}
