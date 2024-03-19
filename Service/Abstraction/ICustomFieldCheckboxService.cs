using TaskMate.DTOs.CustomFieldCheckbox;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldCheckboxService
{
    Task CreateAsync(Guid CustomFieldId);
    Task Update(UpdateCustomFieldCheckboxDto updateCustomFieldCheckboxDto);
}
