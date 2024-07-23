using TaskMate.Entities;

namespace TaskMate.DTOs.DropDownOptionsDTO
{
    public class DropDownOptionsCreateDTO
    {
        public string OptionName { get; set; }
        public string? Color { get; set; }
        public int Order { get; set; }
    }
}
