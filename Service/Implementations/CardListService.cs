using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;
using TaskMate.Context;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardList;
using TaskMate.DTOs.Checkitem;
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
        public async Task<bool> CheckUserAdminRoleInWorkspace(string userId, Guid workspaceId)
        {
            var user = await _appDbContext.WorkspaceUsers
                        .FirstOrDefaultAsync(wu => wu.WorkspaceId == workspaceId && wu.AppUserId == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found in workspace!");
            }

            if (Enum.TryParse<Role>(user.Role, true, out var roleEnum))
            {
                if (roleEnum == Role.GlobalAdmin || roleEnum == Role.Admin)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("The role value in the database is undefined in the Role enum.");
            }
        }
        public async Task CreateAsync(CreateCardListDto createCardListDto)
        {
            var Result = CheckUserAdminRoleInWorkspace(createCardListDto.AppUserId, createCardListDto.WorkspaceId);
            if (await Result == false)
                throw new NotFoundException("No Access");

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

        public async Task Remove(Guid CardlistId, Guid WorkspaceId, string UserId)
        {
            var Result = CheckUserAdminRoleInWorkspace(UserId, WorkspaceId);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var cardList = await _appDbContext.CardLists.FindAsync(CardlistId);
            if (cardList == null)
                throw new NotFoundException("Not Found");

            _appDbContext.CardLists.Remove(cardList);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTitleDto dto)
        {
            var Result = CheckUserAdminRoleInWorkspace(dto.AdminId.ToString(), dto.WorkspaceId);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var cardList = await _appDbContext.CardLists.FindAsync(dto.Id);
            if (cardList == null)
                throw new NotFoundException("Not Found");
            cardList.Title = dto.Title; 
            _appDbContext.CardLists.Update(cardList);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
