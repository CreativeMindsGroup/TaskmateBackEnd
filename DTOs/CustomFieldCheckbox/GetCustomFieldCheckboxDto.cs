namespace TaskMate.DTOs.CustomFieldCheckbox;

public class GetCustomFieldCheckboxDto
{
    public Guid Id { get; set; }
    public bool Check { get; set; }
    public Guid CustomFieldsId { get; set; }

}
