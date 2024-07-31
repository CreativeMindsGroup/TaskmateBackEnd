using TaskMate.DTOs.Checkitem;
using TaskMate.DTOs.Checklist;

namespace TaskMate.Service.Abstraction;

public interface ICheckitemService
{
    Task CreateAsync(CreateCheckitemDto createCheckitemDto);
    Task UpdateAsync(UpdateCheckitemDto updateCheckitemDto);
    Task RemoveAsync(Guid CheckitemId);
    Task<GetCheckItemCountDto> GetChecklistInItemCount(Guid CardId);
    Task UpdateStateOfChecklist(Guid Id, bool State);
    Task EditChecklistItemTitle(UpdateChecklistDto Dto);
}
