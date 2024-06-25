namespace TaskMate.DTOs.CustomFileds
{
    public class RemoveCustomFieldDTO
    {
        public Guid FieldId { get; set; } 
        public string UserId {  get; set; }
        public Guid WorkspaceId { get; set; }

    }
}
