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
    public async Task CreateAsync(CreateCardDto createCardDto)
    {
        if (_appDbContext.CardLists.Where(x => x.Id == createCardDto.CardListId) is null)
            throw new NotFoundException("Not Found Workspace");

        var newcard = _mapper.Map<Card>(createCardDto);
        await _appDbContext.Cards.AddAsync(newcard);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Remove(string AppUserId, Guid CardId)
    {
        var byAdmin = await _userManager.FindByIdAsync(AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                 adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
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

        if (_appDbContext.CardLists.Where(x=>x.Id==updateCardDto.CardListId) is null)
            throw new NotFoundException("Not Found Card List");

        card.Title = updateCardDto.Title;
        card.Description = updateCardDto.Description;
        card.CardListId = updateCardDto.CardListId;

        _appDbContext.Cards.Update(card);
        await _appDbContext.SaveChangesAsync();
    }
}
