namespace TaskMate.DTOs.CustomFieldDropdownOption;

public class GetCustomFieldDropdownOption
{
    public Guid Id { get; set; }
    public string Option { get; set; }
    public string Color { get; set; }
    public Guid CustomFieldsId { get; set; }
}
