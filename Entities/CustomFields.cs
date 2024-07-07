using TaskMate.Entities.Common;
using System;
using System.Collections.Generic;

namespace TaskMate.Entities
{
    public class CustomFields : BaseEntity
    {
        // Relationships
        public Card Card { get; set; }
        public Guid CardId { get; set; }

        public List<CustomFieldsCheckbox>? Checkbox { get; set; }
        public List<CustomFieldsNumber>? Number { get; set; }
        public List<DropDown>? DropDown { get; set; }
    }
}
