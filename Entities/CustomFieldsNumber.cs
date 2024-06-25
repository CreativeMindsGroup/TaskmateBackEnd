using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldsNumber:BaseEntity
{
    public string Title { get; set; }
    public string Number { get; set; }

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
