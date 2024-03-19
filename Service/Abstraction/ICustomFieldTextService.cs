using TaskMate.DTOs.CustomFieldText;
using TaskMate.Entities;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldTextService
{
    Task CreateAsync(Guid CustomFieldId);
    Task Update(UpdateCustomFieldTextDto updateCustomFieldTextDto);
}
