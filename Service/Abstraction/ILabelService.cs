using TaskMate.DTOs.Card;
using TaskMate.DTOs.Label;

namespace TaskMate.Service.Abstraction;

public interface ILabelService
{
    Task CreateAsync(CreateLabelDto createLabelDtos);
    Task<List<GetLabelDto>> GetLabelByCardId(Guid CardId);
    Task CheckBoxCreateAsync(CheckCreateLabelDto createLabelDto);
    Task<List<GetLabelDto>> GetAllLabelInBoard(Guid BoardId);
}
