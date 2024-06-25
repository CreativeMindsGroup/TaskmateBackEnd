using TaskMate.DTOs.Card;

namespace TaskMate.DTOs.CardList
{
    public class CreateCardListDto
    {
        public string Title { get; set; }
        public Guid BoardsId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string AppUserId { get; set; }
    }

    public class UpdateeCardListDto
    {
        public Guid CardListId { get; set; }
        public string Title { get; set; }
        public string AppUserId { get; set; }
    }
    public class GetCardListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public List<GetCardDto> tasks { get; set; } = new List<GetCardDto>();
    }
    public class UpdateCardListOrderDto
    {
        public Guid CardListId { get; set; }
        public int Order { get; set; }
    }
    public class UpdateCardListOrdersDto
    {
        public List<UpdateCardListOrderDto> CardListOrders { get; set; }
    }



}
