using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Card;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CardService : ICardService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public CardService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task AddCardDateAsync(CardAddDatesDto cardAddDatesDto)
    {
        if (cardAddDatesDto.StartDate is null && cardAddDatesDto.EndDate is null)
            throw new Exception("Start Date And End Date not mentioned");
        if (cardAddDatesDto.StartDate > cardAddDatesDto.EndDate)
            throw new Exception("Start date cannot be greater than end date");

        var card = await _appDbContext.Cards.Where(x => x.Id == cardAddDatesDto.CardId).FirstOrDefaultAsync();
        if (card is null)
            throw new NotFoundException("Not Found");

        card.StartDate = cardAddDatesDto.StartDate;
        card.EndDate = cardAddDatesDto.EndDate;

        _appDbContext.Cards.Update(card);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateCardDto createCardDto)
    {
        if (_appDbContext.CardLists.Where(x => x.Id == createCardDto.CardListId) is null)
            throw new NotFoundException("Not Found Workspace");

        var newcard = _mapper.Map<Card>(createCardDto);
        await _appDbContext.Cards.AddAsync(newcard);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto)
    {
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == dragAndDropCardDto.CardId);

        card.CardListId = dragAndDropCardDto.CardListId;
        _appDbContext.Cards.Update(card);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<GetCardDto> GetByIdAsync(Guid CardId)
    {
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == CardId);
        if (card is null) throw new NotFoundException("Not Found");

        var toMapper = _mapper.Map<GetCardDto>(card);
        return toMapper;
    }

    public async Task MoveCardAsync(MoveCard moveCard)
    {
        if (await CheckAdminAsync(moveCard.AppUserId) == false)
            throw new PermisionException("No Access");

        if (await _appDbContext.Boards.FirstOrDefaultAsync(x=>x.Id==moveCard.BoardId) is null)
            throw new NotFoundException("Not Found Exception");

        DragAndDropCardDto dragAndDropCardDto = new()
        {
            CardId = moveCard.CardId,
            CardListId = moveCard.CardListId,
        };
        await DragAndDrop(dragAndDropCardDto);
    }

    public async Task Remove(string AppUserId, Guid CardId)
    {
        if (await CheckAdminAsync(AppUserId) == false)
            throw new PermisionException("No Access");

        var card = await _appDbContext.Cards.Where(x => x.Id == CardId).FirstOrDefaultAsync();
        if (card is null)
            throw new NotFoundException("Not Found");

        _appDbContext.Cards.Remove(card);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateCardDto updateCardDto)
    {
        var card = await _appDbContext.Cards.Where(x => x.Id == updateCardDto.CardId).FirstOrDefaultAsync();
        if (card is null)
            throw new NotFoundException("Not Found");

        card.Title = updateCardDto.Title;
        card.Description = updateCardDto.Description;

        _appDbContext.Cards.Update(card);
        await _appDbContext.SaveChangesAsync();
    }

    private async Task<bool> CheckAdminAsync(string AppUserId)
    {
        var byAdmin = await _userManager.FindByIdAsync(AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                 adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
        {
            return false;
        }
        return true;
    }
}
