using TaskMate.DTOs.CardList;

namespace TaskMate.DTOs.Boards;

public class GetBoardsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Theme { get; set; }
    public Guid WorkspaceId { get; set; }
    public ICollection<GetCardListDto> getCardListDtos { get; set; } = new List<GetCardListDto>();
}
