namespace TaskMate.DTOs.Card
{
    public class UpdateCardDescriptionDto
    {
        public Guid CardId { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public Guid WorkspcaeId { get; set; }
    }
}
