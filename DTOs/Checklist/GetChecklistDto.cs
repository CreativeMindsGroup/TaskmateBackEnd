using TaskMate.DTOs.Checkitem;

namespace TaskMate.DTOs.Checklist;

public class GetChecklistDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int CheckPercentage { get; set; }
    public Guid CardId { get; set; }
    public ICollection<GetCheckitemDto> getCheckitemDtos { get; set; } = new List<GetCheckitemDto>();

}
