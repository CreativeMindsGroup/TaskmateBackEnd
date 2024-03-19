using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldsText:BaseEntity
{
    public string? Text { get; set; } = null;

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
