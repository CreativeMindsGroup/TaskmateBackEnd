using TaskMate.DTOs.CustomFieldDropdownOption;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldDropdownOptionService
{
    Task CreateAsync(List<CreateCustomFieldDropdownOption> createCustomFieldDropdownOptions, Guid CustomFieldId);
    Task RemoveAsync(Guid CustomFieldDropdownOptionId);
    Task Update(UpdateCustomFieldDropdownOption updateCustomFieldDropdownOption);
}
