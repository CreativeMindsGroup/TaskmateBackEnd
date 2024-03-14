using TaskMate.DTOs.CardList;

namespace TaskMate.Service.Abstraction;

public interface ICardListService
{
    Task CreateAsync(CreateCardListDto createCardListDto);
    Task Remove(string AdminId, Guid CardlistId);
    Task UpdateAsync(UpdateeCardListDto updateeCardListDto);
}
