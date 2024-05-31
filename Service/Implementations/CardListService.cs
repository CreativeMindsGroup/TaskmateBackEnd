using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CardList;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations
{
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

            if (await _appDbContext.Boards.FindAsync(createCardListDto.BoardsId) == null)
                throw new NotFoundException("Not Found Workspace");

            // Get the latest order number
            var maxOrder = await _appDbContext.CardLists
                                              .Where(x => x.BoardsId == createCardListDto.BoardsId)
                                              .MaxAsync(x => (int?)x.Order) ?? 0;

            var newCardList = _mapper.Map<CardList>(createCardListDto);
            newCardList.Order = maxOrder + 1;

            await _appDbContext.CardLists.AddAsync(newCardList);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateCardListOrdersAsync(UpdateCardListOrdersDto updateCardListOrdersDto)
        {
            foreach (var update in updateCardListOrdersDto.CardListOrders)
            {
                var cardList = await _appDbContext.CardLists.FindAsync(update.CardListId);
                if (cardList == null)
                    throw new NotFoundException($"Card List with ID {update.CardListId} not found");

                cardList.Order = update.Order;
                _appDbContext.CardLists.Update(cardList);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<GetCardListDto>> GetAllCardListAsync(Guid BoardId)
        {
            var cardLists = await _appDbContext.CardLists
                                               .Where(x => x.BoardsId == BoardId)
                                               .OrderBy(x => x.Order) 
                                               .ToListAsync();
            if (cardLists == null) return null;

            return _mapper.Map<List<GetCardListDto>>(cardLists);
        }


        public async Task<List<GetCardListDto>> GetAllCardListByBoardIdAsync(Guid BoardId)
        {
            var cardLists = await _appDbContext.CardLists
                                               .Where(x => x.BoardsId == BoardId)
                                               .OrderBy(x => x.Order) 
                                               .ToListAsync();
            if (cardLists == null) return null;

            return _mapper.Map<List<GetCardListDto>>(cardLists);
        }

        public async Task Remove(string AdminId, Guid CardlistId)
        {
            var byAdmin = await _userManager.FindByIdAsync(AdminId);

            var adminRol = await _userManager.GetRolesAsync(byAdmin);

            if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                     adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
                throw new PermisionException("No Access");

            var cardList = await _appDbContext.CardLists.FindAsync(CardlistId);
            if (cardList == null)
                throw new NotFoundException("Not Found");

            _appDbContext.CardLists.Remove(cardList);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateeCardListDto updateCardListDto)
        {
            var byAdmin = await _userManager.FindByIdAsync(updateCardListDto.AppUserId);

            var adminRol = await _userManager.GetRolesAsync(byAdmin);

            if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
                     adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
                throw new PermisionException("No Access");

            var cardList = await _appDbContext.CardLists.FindAsync(updateCardListDto.CardListId);
            if (cardList == null)
                throw new NotFoundException("Not Found");

            _mapper.Map(updateCardListDto, cardList);
            _appDbContext.CardLists.Update(cardList);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
