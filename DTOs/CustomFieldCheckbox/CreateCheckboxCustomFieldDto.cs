namespace TaskMate.DTOs.CustomFieldCheckbox
{
    public class CreateCheckboxCustomFieldDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Check { get; set; }
        public Guid CardId { get; set; }
        public string UserId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
