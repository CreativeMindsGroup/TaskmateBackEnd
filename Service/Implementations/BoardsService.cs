using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMate.Context;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.Checkitem;
using TaskMate.DTOs.Workspace;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class BoardsService : IBoardsService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWorkspaceService _workspaceService;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public BoardsService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper, IWorkspaceService workspaceService, IAuthService authService, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
        _workspaceService = workspaceService;
        _authService = authService;
        _configuration = configuration;
    }
    public async Task<bool> CheckUserAdminRoleInWorkspace(string userId, Guid workspaceId,bool isMemberAllowed)
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
            else if (roleEnum == Role.Member && isMemberAllowed)
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
    public async Task CreateAsync(CreateBoardsDto createBoardsDto)
    {
        var Result = CheckUserAdminRoleInWorkspace(createBoardsDto.AppUserId, createBoardsDto.WorkspaceId,false);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var workspace = await _appDbContext.Workspaces
                               .FirstOrDefaultAsync(w => w.Id == createBoardsDto.WorkspaceId);
        if (workspace == null)
            throw new NotFoundException("Workspace not found!");
        var newBoard = _mapper.Map<Boards>(createBoardsDto);
        await _appDbContext.Boards.AddAsync(newBoard);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<GetBoardsDto>> GetAllAsync(string AppUserId, Guid WorkspaceId)
    {

        var appUser = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == AppUserId);
        if (appUser is null) throw new NotFoundException("Not Found");

        var workspaces = await _workspaceService.GetAllAsync(AppUserId);
        if (workspaces is null) return null;

        bool isTrue = false;
        foreach (var item in workspaces)
            if (item.Id == WorkspaceId) isTrue = true;


        if (isTrue)
        {
            var WokrspaceInBoards = await _appDbContext.Boards.Where(x => x.WorkspaceId == WorkspaceId).ToListAsync();
            return _mapper.Map<List<GetBoardsDto>>(WokrspaceInBoards);
        }
        else return null;
    }
    public async Task<GetBoardsDto> GetByIdAsync(Guid boardId)
    {
        var board = await _appDbContext.Boards
            .Where(b => b.Id == boardId)
            .Include(b => b.CardLists)
                .ThenInclude(cl => cl.Cards)
                    .ThenInclude(c => c.AppUsersCards)
                        .ThenInclude(ac => ac.AppUser)  // Include AppUsers for Cards
            .Include(b => b.CardLists)
                .ThenInclude(cl => cl.Cards)
                    .ThenInclude(c => c.CustomFields)  // Include CustomFields for Cards
            .FirstOrDefaultAsync();

        if (board == null)
            throw new NotFoundException("Board not found.");

        // Perform in-memory ordering
        foreach (var cardList in board.CardLists)
        {
            cardList.Cards = cardList.Cards.Where(c => !c.isArchived).OrderBy(c => c.Order).ToList();
        }

        board.CardLists = board.CardLists.OrderBy(cl => cl.Order).ToList();

        // Retrieve checklist counts for each card
        var cardIds = board.CardLists.SelectMany(cl => cl.Cards.Select(c => c.Id)).ToList();
        var checklistCounts = await GetChecklistInItemCounts(cardIds);

        // Map to DTO and set checklist counts
        var boardDto = _mapper.Map<GetBoardsDto>(board);

        foreach (var cardListDto in boardDto.cardLists)
        {
            foreach (var cardDto in cardListDto.tasks)
            {
                if (checklistCounts.TryGetValue(cardDto.Id, out var countDto))
                {
                    cardDto.ChecklistDoneCount = countDto.Done;
                    cardDto.ChecklistTotalCount = countDto.Total;
                }
            }
        }

        return boardDto;
    }

    public async Task<Dictionary<Guid, GetCheckItemCountDto>> GetChecklistInItemCounts(IEnumerable<Guid> cardIds)
    {
        var checklists = await _appDbContext.Checklists
            .Include(x => x.Checkitems)
            .Where(x => cardIds.Contains(x.CardId))
            .ToListAsync();

        if (!checklists.Any())
            return new Dictionary<Guid, GetCheckItemCountDto>();

        var cardCounts = new Dictionary<Guid, (int doneCount, int totalCount)>();

        foreach (var checklist in checklists)
        {
            if (!cardCounts.ContainsKey(checklist.CardId))
            {
                cardCounts[checklist.CardId] = (0, 0);
            }

            var (currentDoneCount, currentTotalCount) = cardCounts[checklist.CardId];
            cardCounts[checklist.CardId] = (
                currentDoneCount + checklist.Checkitems.Count(c => c.Check),
                currentTotalCount + checklist.Checkitems.Count
            );
        }

        var result = cardCounts.ToDictionary(
            kvp => kvp.Key,
            kvp => new GetCheckItemCountDto
            {
                Done = kvp.Value.doneCount,
                Total = kvp.Value.totalCount
            }
        );

        return result;
    }

    public async Task<List<GetArchivedCardDto>> GetArchivedCardsInBoard(Guid boardId)
    {
        // Filter by CardList.BoardId and IsArchived
        var archivedCards = await _appDbContext.Cards
            .Include(c => c.CardList)
            .Where(c => c.CardList.BoardsId == boardId && c.isArchived)
            .ToListAsync();

        // If no archived cards are found, return an empty list instead of throwing an exception
        if (archivedCards == null || !archivedCards.Any())
        {
            return new List<GetArchivedCardDto>();
        }

        return _mapper.Map<List<GetArchivedCardDto>>(archivedCards);
    }



    public async Task Remove(string AdminId, Guid BoardId, Guid WorkspaceId)
    {
        var Result = CheckUserAdminRoleInWorkspace(AdminId, WorkspaceId, false);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var board = await _appDbContext.Boards
                               .FirstOrDefaultAsync(w => w.Id == BoardId);
        if (board == null)
            throw new NotFoundException("Board not found!");
        _appDbContext.Boards.Remove(board);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(UpdateBoardsDto updateBoardsDto)
    {
        var Result = CheckUserAdminRoleInWorkspace(updateBoardsDto.AppUserId, updateBoardsDto.WorkspaceId, false);
        if (await Result == false)
            throw new NotFoundException("No Access");
        // Check if workspace exists
        var board = await _appDbContext.Boards
                               .FirstOrDefaultAsync(w => w.Id == updateBoardsDto.BoardId);
        if (board == null)
            throw new NotFoundException("Board not found!");

        board.Title = updateBoardsDto.Title;
        board.Theme = updateBoardsDto.Theme;

        _appDbContext.Boards.Update(board);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task UpdateCardPositionAsync(Guid cardId, Guid sourceColumnId, Guid destinationColumnId, int newPosition,Guid workspaceId ,string userId)
    {
        var Result = CheckUserAdminRoleInWorkspace(userId, workspaceId, true);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var card = await _appDbContext.Cards
                                      .Include(c => c.CardList)
                                      .SingleOrDefaultAsync(c => c.Id == cardId);

        if (card == null)
        {
            throw new NotFoundException("Card not found");
        }

        if (sourceColumnId != destinationColumnId)
        {
            var newColumn = await _appDbContext.CardLists.FindAsync(destinationColumnId);
            if (newColumn == null)
            {
                throw new NotFoundException("Destination column not found");
            }

            card.CardListId = destinationColumnId;
        }
        var cards = await _appDbContext.Cards
                                       .Where(c => c.CardListId == destinationColumnId)
                                       .OrderBy(c => c.Order)
                                       .ToListAsync();

        if (sourceColumnId == destinationColumnId)
        {
            cards.Remove(card);
        }

        cards.Insert(newPosition, card);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Order = i;
        }

        await _appDbContext.SaveChangesAsync();
    }
    public async Task UpdateCardListPositionAsync(Guid boardId, List<Guid> newOrder)
    {
        var board = await _appDbContext.Boards
                                       .Include(b => b.CardLists)
                                       .SingleOrDefaultAsync(b => b.Id == boardId);
        if (board == null)
        {
            throw new NotFoundException("Board not found.");
        }
        var cardLists = board.CardLists.ToList();
        if (newOrder.Except(cardLists.Select(cl => cl.Id)).Any())
        {
            throw new InvalidOperationException("One or more card lists in the new order do not exist on this board.");
        }
        var orderMap = newOrder.Select((id, index) => new { id, index }).ToDictionary(x => x.id, x => x.index);
        foreach (var list in cardLists)
        {
            if (orderMap.TryGetValue(list.Id, out var newIndex))
            {
                list.Order = newIndex;
            }
        }
        await _appDbContext.SaveChangesAsync();
    }
}
