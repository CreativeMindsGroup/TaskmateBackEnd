using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldsCheckbox:BaseEntity
{
    public string Title { get; set; }
    public bool Check { get; set; } = false;

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
