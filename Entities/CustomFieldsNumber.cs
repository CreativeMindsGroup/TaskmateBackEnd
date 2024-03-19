using TaskMate.Entities.Common;

namespace TaskMate.Entities;

public class CustomFieldsNumber:BaseEntity
{
    public decimal? Number { get; set; }

    //Rellations
    public CustomFields CustomFields { get; set; }
    public Guid CustomFieldsId { get; set; }
}
