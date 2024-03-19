namespace TaskMate.DTOs.CustomFieldNumber;

public class GetCustomFieldNumberDto
{
    public Guid Id { get; set; }
    public decimal? Number { get; set; }
    public Guid CustomFieldsId { get; set; }

}
