using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CardList;
using TaskMate.DTOs.Checklist;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class ChecklistService : IChecklistService
{
    private readonly AppDbContext _appDbContext;
    private readonly ICheckitemService _checkitemService;
    private readonly IMapper _mapper;
    public ChecklistService(AppDbContext appDbContext, IMapper mapper, ICheckitemService checkitemService)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _checkitemService = checkitemService;

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

    public async Task CreateAsync(CreateChecklistDto createChecklistDto)
    {
        var Result = CheckUserAdminRoleInWorkspace(createChecklistDto.AppUserId, createChecklistDto.WorkspaceId);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == createChecklistDto.CardId);
        if (card is null) throw new NotFoundException("Not Found");
        var newCheckList = _mapper.Map<Checklist>(createChecklistDto);

        await _appDbContext.Checklists.AddAsync(newCheckList);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task<List<GetChecklistDto>> GetAllAsync(Guid CardId)
    {
        var checklists = await _appDbContext.Checklists
            .Include(x => x.Checkitems.OrderBy(c => c.Order)) 
            .Where(x => x.CardId == CardId)
            .ToListAsync();

        if (!checklists.Any())
            throw new NotFoundException("No checklists found");

        foreach (var checklist in checklists)
        {
            var itemCount = checklist.Checkitems.Count;
            if (itemCount > 0)
            {
                var completedCount = checklist.Checkitems.Count(i => i.Check);
                var percentage = 100 * completedCount / itemCount;
                checklist.CheckPercentage = percentage > 99 ? 100 : percentage;
            }
        }

        // Map and return
        return _mapper.Map<List<GetChecklistDto>>(checklists);
    }

    public async Task RemoveAsync(Guid CheckListId, Guid WorkspaceId, string UserId)
    {
        var Result = CheckUserAdminRoleInWorkspace(UserId, WorkspaceId);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var checklist = await _appDbContext.Checklists.FirstOrDefaultAsync(x => x.Id == CheckListId);
        if (checklist is null) throw new NotFoundException("Not Found");
        _appDbContext.Checklists.Remove(checklist);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateChecklistDto updateChecklistDto)
    {
        var checklist = await _appDbContext.Checklists.FirstOrDefaultAsync(x => x.Id == updateChecklistDto.Id);
        if (checklist is null) throw new NotFoundException("Not Found");

        checklist.Name = updateChecklistDto.Name;

        _appDbContext.Checklists.Update(checklist);
        await _appDbContext.SaveChangesAsync();
    }
}
