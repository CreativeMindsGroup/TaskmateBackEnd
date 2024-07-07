using TaskMate.Entities;

namespace TaskMate.DTOs.DropDownOptionsDTO
{
    public class GetDropDownDto
    {
        public Guid Id { get; set; }    
        public string Title { get; set; }
        public Guid SelectedId { get; set; }
        public string? OptionName { get; set; }
        public string? Color { get; set; }
        public List<GetDropDownOptionsDTO>? DropDownOptions { get; set; }
    }
}
