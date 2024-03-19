using TaskMate.DTOs.Checklist;

namespace TaskMate.Service.Abstraction;

public interface IChecklistService
{
    Task CreateAsync(CreateChecklistDto createChecklistDto);
    Task<List<GetChecklistDto>> GetAllAsync(Guid CardId);
    Task UpdateAsync(UpdateChecklistDto updateChecklistDto);
    Task RemoveAsync(Guid ChecklistId);
}
