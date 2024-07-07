using TaskMate.Entities.Common;
using System;
using System.Collections.Generic;

namespace TaskMate.Entities
{
    public class DropDown : BaseEntity
    {
        public string Title { get; set; }
        public Guid SelectedId { get; set; }
        public string? OptionName { get; set; }
        public string? Color { get; set; }

        // Relationships
        public CustomFields CustomFields { get; set; }
        public Guid CustomFieldsId { get; set; }

        public List<DropDownOptions>? DropDownOptions { get; set; }
    }
}
