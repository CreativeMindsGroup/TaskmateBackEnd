using TaskMate.DTOs.Checkitem;

namespace TaskMate.Service.Abstraction;

public interface ICheckitemService
{
    Task CreateAsync(CreateCheckitemDto createCheckitemDto);
    Task UpdateAsync(UpdateCheckitemDto updateCheckitemDto);
    Task RemoveAsync(Guid CheckitemId);
    Task<GetCheckItemCountDto> GetChecklistInItemCount(Guid ChecklistId);
}
