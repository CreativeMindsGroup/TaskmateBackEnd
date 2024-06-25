using TaskMate.Entities.Common;
using TaskMate.Helper.Enum.CustomFields;

namespace TaskMate.Entities;

public class CustomFields:BaseEntity
{
    //Rellations
    public Card Card { get; set; }
    public Guid CardId { get; set; }
    public List<CustomFieldsCheckbox>? Checkbox { get; set; }
    public List<CustomFieldsNumber>? Number { get; set; }

}
