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
    public string Title { get; set; }
    public CustomFieldsType Type { get; set; }
    public Guid CardId { get; set; }
    public List<GetCustomFieldDropdownOption>? GetCustomFieldDropdownOptions { get; set; }
    public GetCustomFieldDateDto GetCustomFieldDateDto { get; set; }
    public GetCustomFieldCheckboxDto GetCustomFieldCheckboxDto { get; set; }
    public GetCustomFieldNumberDto GetCustomFieldNumberDto { get; set; }
    public GetCustomFieldTextDto GetCustomFieldTextDto { get; set; }

}
