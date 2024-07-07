using TaskMate.Entities;

namespace TaskMate.DTOs.DropDownOptionsDTO
{
    public class GetDropDownOptionsDTO
    {
        public Guid Id { get; set; }  
        public string OptionName { get; set; }
        public string? Color { get; set; }
        public int Order { get; set; }
    }
}
