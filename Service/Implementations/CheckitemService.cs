using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Checkitem;
using TaskMate.DTOs.Checklist;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskMate.Service.Implementations;

public class CheckitemService : ICheckitemService
{

    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CheckitemService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
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
    public async Task CreateAsync(CreateCheckitemDto createCheckitemDto)
    {
        if (await _appDbContext.Checklists.FirstOrDefaultAsync(x => x.Id == createCheckitemDto.ChecklistId) is null)
            throw new NotFoundException("Not Found");

        var newCheckItem = _mapper.Map<Checkitem>(createCheckitemDto);

        await _appDbContext.Checkitems.AddAsync(newCheckItem);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task<GetCheckItemCountDto> GetChecklistInItemCount(Guid CardId)
    {
        var checklists = await _appDbContext.Checklists
            .Include(x => x.Checkitems)
            .Where(x => x.CardId == CardId)
            .ToListAsync();

        if (!checklists.Any())
            throw new NotFoundException("No checklists found");

        int totalDoneCount = 0;
        int totalItemCount = 0;

        // Aggregate counts across all checklists
        foreach (var checklist in checklists)
        {
            totalDoneCount += checklist.Checkitems.Count(c => c.Check);
            totalItemCount += checklist.Checkitems.Count;
        }

        // Return the aggregated counts in a single DTO
        return new GetCheckItemCountDto
        {
            Done = totalDoneCount,
            Total = totalItemCount
        };
    }

    public async Task RemoveAsync(Guid CheckitemId)
    {
        var checkitem = await _appDbContext.Checkitems.FirstOrDefaultAsync(x => x.Id == CheckitemId);
        if (checkitem is null)
            throw new NotFoundException("Not Found");

        _appDbContext.Checkitems.Remove(checkitem);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateCheckitemDto updateCheckitemDto)
    {
        var checkitem = await _appDbContext.Checkitems.FirstOrDefaultAsync(x => x.Id == updateCheckitemDto.Id);
        if (checkitem is null)
            throw new NotFoundException("Not Found");

        if (updateCheckitemDto.Text is null)
            checkitem.Text = updateCheckitemDto.Text;
        if (updateCheckitemDto.Text is null)
            checkitem.DueDate = updateCheckitemDto.DueDate;
        if (updateCheckitemDto.Text is null)
            checkitem.Check = updateCheckitemDto.Check;

        _appDbContext.Update(checkitem);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task UpdateStateOfChecklist(Guid Id, bool State)
    {
        var checkitem = await _appDbContext.Checkitems.FirstOrDefaultAsync(x => x.Id == Id);
        if (checkitem is null)
            throw new NotFoundException("Not Found");
        checkitem.Check = State;
        _appDbContext.Update(checkitem);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task EditChecklistItemTitle(UpdateChecklistDto Dto)
    {
        var Result = CheckUserAdminRoleInWorkspace(Dto.UserId.ToString(), Dto.WorkspaceId);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var checkitem = await _appDbContext.Checkitems.FirstOrDefaultAsync(x => x.Id == Dto.Id);
        if (checkitem is null)
            throw new NotFoundException("Not Found");
        checkitem.Text = Dto.Title;
        _appDbContext.Update(checkitem);
        await _appDbContext.SaveChangesAsync();
    }

}
 