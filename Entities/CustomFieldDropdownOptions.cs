using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldDropdownOptions:BaseEntity
{
    public string Option { get; set; }
    public string Color { get; set; } = "transparent";

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
