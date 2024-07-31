using TaskMate.DTOs.Checklist;

namespace TaskMate.Service.Abstraction;

public interface IChecklistService
{
    Task CreateAsync(CreateChecklistDto createChecklistDto);
    Task<List<GetChecklistDto>> GetAllAsync(Guid CardId);
    Task RemoveAsync(Guid CheckListId, Guid WorkspaceId, string UserId);
    Task EditChecklistTitle(UpdateChecklistDto Dto);
}
