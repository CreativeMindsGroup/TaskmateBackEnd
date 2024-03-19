using TaskMate.Entities.Common;

namespace TaskMate.Entities;

    public class Card:BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Rellations
        public CardList CardList { get; set; }
        public Guid CardListId { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<LabelCard>? LabelsCards { get; set; }
        public List<Checklist>? Checklists { get; set; }
        public List<CustomFields>? CustomFields { get; set; }

    }
