namespace TaskMate.DTOs.Card
{
    public class CreateCardDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid CardListId { get; set; }
        public int Order { get; set; }  // Add this line
    }
    public class UpdateCardDto
    {
        public Guid CardId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }

    public class GetCardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }

    public class CardAddDatesDto
    {
        public Guid CardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DragAndDropCardDto
    {
        public Guid CardId { get; set; }
        public Guid CardListId { get; set; }
    }

    public class ReorderCardsDto
    {
        public List<CardOrderDto> CardOrders { get; set; }
    }

    public class CardOrderDto
    {
        public Guid CardId { get; set; }
        public int Order { get; set; }
    }
}
