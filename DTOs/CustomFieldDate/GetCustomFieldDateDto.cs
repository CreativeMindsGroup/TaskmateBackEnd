namespace TaskMate.DTOs.CustomFieldDate;

public class GetCustomFieldDateDto
{
    public Guid Id { get; set; }
    public DateTime? DateTime { get; set; }
    public Guid CustomFieldsId { get; set; }

}
