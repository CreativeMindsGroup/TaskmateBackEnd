using TaskMate.Entities.Common;
using System;

namespace TaskMate.Entities
{
    public class DropDownOptions : BaseEntity
    {
        public string OptionName { get; set; }
        public string? Color { get; set; }
        public int Order { get; set; }

        // Relationships
        public DropDown? DropDown { get; set; }
        public Guid? DropDownId { get; set; }
    }
}
