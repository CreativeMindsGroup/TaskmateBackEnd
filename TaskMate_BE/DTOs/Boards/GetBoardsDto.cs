using TaskMate.DTOs.CardList;

namespace TaskMate.DTOs.Boards;

public class GetBoardsDto
{
    public string Title { get; set; }
    public Guid WorkspaceId { get; set; }
    public ICollection<GetCardListDto> getCardListDtos { get; set; } = new List<GetCardListDto>();
}
