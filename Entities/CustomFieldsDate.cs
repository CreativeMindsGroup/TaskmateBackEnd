using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldsDate:BaseEntity
{
    public DateTime? DateTime { get; set; }

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
