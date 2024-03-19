using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.CardList;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CardListService : ICardListService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public CardListService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task CreateAsync(CreateCardListDto createCardListDto)
    {
        var byAdmin = await _userManager.FindByIdAsync(createCardListDto.AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
            adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");

        if (_appDbContext.Boards.Where(x => x.Id == createCardListDto.BoardsId) is null)
            throw new NotFoundException("Not Found Workspace");

        var newcardList = _mapper.Map<CardList>(createCardListDto);
        await _appDbContext.CardLists.AddAsync(newcardList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<GetCardListDto>> GetAllCardListAsync(Guid BoardId)
    {
        var board = await _appDbContext.CardLists.Where(x=>x.BoardsId==BoardId).ToListAsync();
        if (board is null) return null;

        return _mapper.Map<List<GetCardListDto>>(board);
    }

    public async Task Remove(string AdminId, Guid CardlistId)
    {
        var byAdmin = await _userManager.FindByIdAsync(AdminId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                 adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");

        var cardlist = await _appDbContext.CardLists.Where(x => x.Id == CardlistId).FirstOrDefaultAsync();
        if (cardlist is null)
            throw new NotFoundException("Not Found");

        _appDbContext.CardLists.Remove(cardlist);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateeCardListDto updateeCardListDto)
    {
        var byAdmin = await _userManager.FindByIdAsync(updateeCardListDto.AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                 adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");

        var cardList = await _appDbContext.CardLists.Where(x => x.Id == updateeCardListDto.CardListId).FirstOrDefaultAsync();
        if (cardList is null)
            throw new NotFoundException("Not Found");

        cardList.Title = updateeCardListDto.Title;
        _appDbContext.CardLists.Update(cardList);
        await _appDbContext.SaveChangesAsync();
    }
}
