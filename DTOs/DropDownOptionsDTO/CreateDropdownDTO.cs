namespace TaskMate.DTOs.DropDownOptionsDTO
{
    public class CreateDropdownDTO
    {
        public string Title { get; set; }
        public Guid CardId { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid BoardId { get; set; }
        public Guid UserId { get; set; }
        public List<DropDownOptionsCreateDTO> DropDownOptions { get; set; }
    }
}
