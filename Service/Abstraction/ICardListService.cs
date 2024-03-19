using TaskMate.DTOs.CardList;

namespace TaskMate.Service.Abstraction;

public interface ICardListService
{
    Task CreateAsync(CreateCardListDto createCardListDto);
    Task Remove(string AdminId, Guid CardlistId);
    Task UpdateAsync(UpdateeCardListDto updateeCardListDto);


    /// <summary>
    /// Bu Methodun meqsedi her hansi bir cardi baska boarda ve yaxud oz boardindaki her hansi CardList'e gonderemek ucun
    /// BoardId qebul edir ve sene Yalniz Qebul etdiyin BoardId olan Board'da hansi CardList'ler var onun adini ve Id'sini getirecek !!!
    /// </summary>
    /// <returns></returns>
    Task<List<GetCardListDto>> GetAllCardListAsync(Guid BoardId);
}
