using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Card;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations
{
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
            if (cardAddDatesDto.StartDate == null || cardAddDatesDto.EndDate == null)
                throw new Exception("Start Date and End Date must be mentioned");

            if (cardAddDatesDto.StartDate > cardAddDatesDto.EndDate)
                throw new Exception("Start Date cannot be greater than End Date");

            var card = await _appDbContext.Cards.FindAsync(cardAddDatesDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.StartDate = cardAddDatesDto.StartDate;
            card.EndDate = cardAddDatesDto.EndDate;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateCardDto createCardDto)
        {
            if (!await _appDbContext.CardLists.AnyAsync(x => x.Id == createCardDto.CardListId))
                throw new NotFoundException("Card List not found");

            // Get the latest order number
            var maxOrder = await _appDbContext.Cards
                                              .Where(x => x.CardListId == createCardDto.CardListId)
                                              .MaxAsync(x => (int?)x.Order) ?? 0;

            var newCard = _mapper.Map<Card>(createCardDto);
            newCard.Order = maxOrder + 1;  // Set the order

            await _appDbContext.Cards.AddAsync(newCard);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto)
        {
            var card = await _appDbContext.Cards.FindAsync(dragAndDropCardDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            // Adjust the order of the card being moved
            var targetCardList = await _appDbContext.CardLists
                .Include(cl => cl.Cards)
                .FirstOrDefaultAsync(cl => cl.Id == dragAndDropCardDto.CardListId);

            if (targetCardList == null)
                throw new NotFoundException("Target Card List not found");

            var maxOrder = targetCardList.Cards.Max(c => (int?)c.Order) ?? 0;
            card.Order = maxOrder + 1;
            card.CardListId = dragAndDropCardDto.CardListId;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<GetCardDto> GetByIdAsync(Guid cardId)
        {
            var card = await _appDbContext.Cards.FindAsync(cardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            return _mapper.Map<GetCardDto>(card);
        }


        public async Task Remove(string appUserId, Guid cardId)
        {
            if (!await CheckAdminAsync(appUserId))
                throw new PermisionException("No Access");

            var card = await _appDbContext.Cards.FindAsync(cardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            var cardListId = card.CardListId;
            var order = card.Order;

            _appDbContext.Cards.Remove(card);
            await _appDbContext.SaveChangesAsync();

            // Update the order of the remaining cards in the same list
            var remainingCards = await _appDbContext.Cards
                .Where(c => c.CardListId == cardListId && c.Order > order)
                .ToListAsync();

            foreach (var remainingCard in remainingCards)
            {
                remainingCard.Order--;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<GetCardDto>> GetAllCardsByBoardIdAsync(Guid boardId)
        {
            var cards = await _appDbContext.Cards
                .Include(c => c.CardList)
                .Where(c => c.CardList.BoardsId == boardId)
                .OrderBy(c => c.Order)  // Ensure the cards are ordered by the Order property
                .ToListAsync();

            return _mapper.Map<List<GetCardDto>>(cards);
        }
        public async Task<List<GetCardDto>> GetAllCardsByCardListIdAsync(Guid cardListId)
        {
            var cards = await _appDbContext.Cards
                .Where(c => c.CardListId == cardListId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            return _mapper.Map<List<GetCardDto>>(cards);
        }

        public async Task UpdateAsync(UpdateCardDto updateCardDto)
        {
            var card = await _appDbContext.Cards.FindAsync(updateCardDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.Title = updateCardDto.Title;
            card.Description = updateCardDto.Description;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }


        private async Task<bool> CheckAdminAsync(string appUserId)
        {
            var user = await _userManager.FindByIdAsync(appUserId);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Contains(Role.GlobalAdmin.ToString()) || roles.Contains(Role.Admin.ToString());
        }

        public async Task ReorderCardsAsync(ReorderCardsDto reorderCardsDto)
        {
            foreach (var cardOrder in reorderCardsDto.CardOrders)
            {
                var card = await _appDbContext.Cards.FindAsync(cardOrder.CardId);
                if (card != null)
                {
                    card.Order = cardOrder.Order;
                    _appDbContext.Cards.Update(card);
                }
            }
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateCardDescriptionAsync(UpdateCardDescriptionDto updateCardDescriptionDto)
        {
            var card = await _appDbContext.Cards.FindAsync(updateCardDescriptionDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.Description = updateCardDescriptionDto.Description;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task ChangeCoverColorAsync(Guid cardId, string coverColor)
        {
            var card = await _appDbContext.Cards.FindAsync(cardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.CoverColor = coverColor;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }


    }
}
