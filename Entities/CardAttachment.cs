namespace TaskMate.Entities
{
    public class CardAttachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid CardId { get; set; }
        public Card Card { get; set; }
    }
}
