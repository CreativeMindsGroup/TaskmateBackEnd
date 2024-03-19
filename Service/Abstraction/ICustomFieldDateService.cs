using TaskMate.DTOs.CustomFieldDate;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldDateService
{
    Task CreateAsync(Guid CustomFieldId);
    Task Update(UpdateCustomFieldDateDto updateCustomFieldDateDto);
    Task Remove(Guid CustomFieldId);
}
