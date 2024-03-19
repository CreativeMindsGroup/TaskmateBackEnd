using TaskMate.DTOs.CustomField;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldsService
{
    Task CreateAsync(CreateCustomFieldDto createCustomFieldDto);
    Task<List<GetCustomFieldDto>> GetCardInCustomFieldAsync(Guid CardId);
    Task RemoveAsync(Guid CustomFieldId);
    Task Update(UpdateCustomFieldDto updateCustomFieldDto);
}
