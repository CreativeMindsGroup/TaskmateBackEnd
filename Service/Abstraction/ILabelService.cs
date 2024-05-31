using TaskMate.DTOs.Card;
using TaskMate.DTOs.Label;

namespace TaskMate.Service.Abstraction;

public interface ILabelService
{
    Task CreateAsync(CreateLabelDto createLabelDtos);
}
