using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.Entities;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldNumberService
{
    Task CreateAsync(Guid CustomFieldId);
    Task Update(UpdateCustomFieldsNumberDto updateCustomFieldsNumberDto);
}
