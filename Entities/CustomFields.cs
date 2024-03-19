using TaskMate.Entities.Common;
using TaskMate.Helper.Enum.CustomFields;

namespace TaskMate.Entities;

public class CustomFields:BaseEntity
{
    public string Title { get; set; }
    public CustomFieldsType Type { get; set; }

    //Rellations
    public Card Card { get; set; }
    public Guid CardId { get; set; }
    public List<CustomFieldDropdownOptions>? CustomFieldDropdownOptions { get; set; }
    public CustomFieldsDate CustomFieldsDates { get; set; }
    public CustomFieldsCheckbox CustomFieldsCheckboxes { get; set; }
    public CustomFieldsNumber CustomFieldsNumbers { get; set; }
    public CustomFieldsText CustomFieldsTexts { get; set; }
}
