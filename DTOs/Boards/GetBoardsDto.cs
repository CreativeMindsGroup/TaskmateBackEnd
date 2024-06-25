using TaskMate.DTOs.CardList;

public class GetBoardsDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Theme { get; set; }
    public Guid WorkspaceId { get; set; }
    public List<GetCardListDto> cardLists { get; set; } = new List<GetCardListDto>();
}
