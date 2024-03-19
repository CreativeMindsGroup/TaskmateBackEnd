using TaskMate.DTOs.CustomFieldDropdownOption;
using TaskMate.Helper.Enum.CustomFields;

namespace TaskMate.DTOs.CustomField;

public class CreateCustomFieldDto
{
    public string Title { get; set; }
    public CustomFieldsType Type { get; set; }
    public Guid CardId { get; set; }
    public List<CreateCustomFieldDropdownOption>? CreateCustomFieldDropdownOptions { get; set; }

}
