using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldDate;
using TaskMate.DTOs.CustomFieldDropdownOption;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFieldText;
using TaskMate.Entities;
using TaskMate.Helper.Enum.CustomFields;

namespace TaskMate.DTOs.CustomField;

public class GetCustomFieldDto
{
    public Guid Id { get; set; }
    public List<GetCustomFieldCheckboxDto>? CheckboxDto { get; set; }
    public List<GetCustomFiledNumber>? NumberDto { get; set; }

}
