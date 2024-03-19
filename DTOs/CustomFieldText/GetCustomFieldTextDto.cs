namespace TaskMate.DTOs.CustomFieldText;

public class GetCustomFieldTextDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid CustomFieldsId { get; set; }
}
