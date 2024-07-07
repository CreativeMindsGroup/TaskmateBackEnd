using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFileds;
using TaskMate.DTOs.DropDownOptionsDTO;
using TaskMate.Exceptions;

namespace TaskMate.Service.Abstraction;

public interface ICustomFieldsService
{
    Task CreateChecklistAsync(CreateCheckboxCustomFieldDto Dto);
    Task CreateNumberAsync(CustomFieldNumberDto Dto);
    Task RemoveAsync(Guid CustomFieldId);
    Task<GetCustomFieldDto> GetCustomFieldsAsync(Guid cardId);
    Task RemoveCustomField(RemoveCustomFieldDTO dto);
    Task UpdateChecklist(bool value, Guid id);
    Task UpdateCustomField(string value, Guid Id);
    Task CreateDropdown(CreateDropdownDTO dto);
    Task RemoveDropDown(Guid DropdownId);
    Task SetOptionToDropdown(Guid DropDownId, Guid DropdownOptionId);

}