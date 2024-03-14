using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Workspace;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class WorkspaceService : IWorkspaceService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public WorkspaceService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper, IAuthService authService)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task AddUserWorkspace(AddUserWorkspace addUserWorkspace)
    {
        var byAdmin = await _userManager.FindByIdAsync(addUserWorkspace.AdminId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
            adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");

        if (_userManager.FindByIdAsync(addUserWorkspace.AppUserId) is null)
            throw new NotFoundException("Not Found User");

        var newWorkspaceUser = new WorkspaceUser()
        {
            AppUserId = addUserWorkspace.AppUserId,
            WorkspaceId = addUserWorkspace.WorkspaceId,
        };

        await _appDbContext.WorkspaceUsers.AddAsync(newWorkspaceUser);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(CreateWorkspaceDto createWorkspaceDto)
    {
        var byGlobalAdmin = await _userManager.FindByIdAsync(createWorkspaceDto.AppUserId);
        var adminRol = await _userManager.GetRolesAsync(byGlobalAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString())
            throw new PermisionException("No Access");

        var newWokspace = _mapper.Map<Workspace>(createWorkspaceDto);
        await _appDbContext.Workspaces.AddAsync(newWokspace);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<GetWorkspaceDto>> GetAllAsync(string AppUserId)
    {
        var byAdmin = await _userManager.FindByIdAsync(AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() == Role.GlobalAdmin.ToString() ||
            adminRol.FirstOrDefault().ToString() == Role.Admin.ToString())
        {
            var AllWokrspace = await _appDbContext.Workspaces.ToListAsync();
            var AllWokrspaceToMapper = _mapper.Map<List<GetWorkspaceDto>>(AllWokrspace);

            return AllWokrspaceToMapper;
        }

        var UserConnectWorkspace = await _appDbContext.Workspaces
                                            .Include(w => w.WorkspaceUsers)
                                            .ThenInclude(wu => wu.AppUser)
                                            .Where(w => w.WorkspaceUsers.Any(wu => wu.AppUserId == AppUserId))
                                            .ToListAsync();

        var UserConnrctWorkspaceToMapper = _mapper.Map<List<GetWorkspaceDto>>(UserConnectWorkspace);
        return UserConnrctWorkspaceToMapper;
    }

    public async Task<GetWorkspaceDto> GetByIdAsync(Guid WorspaceId)
    {
        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == WorspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Not Found");

        return _mapper.Map<GetWorkspaceDto>(worksPace);
    }

    public async Task Remove(string AppUserId, Guid WokspaceId)
    {
        var byGlobalAdmin = await _userManager.FindByIdAsync(AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byGlobalAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString())
            throw new PermisionException("No Access");

        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == WokspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Not Found");

        _appDbContext.Workspaces.Remove(worksPace);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task ShareLinkBoardToUser(LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        var byAdmin = await _userManager.FindByIdAsync(linkShareToWorkspaceDto.AdminId);

        var adminRol = await _userManager.GetRolesAsync(byAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
            adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");

        var tokenResponse = await _authService.ShareLinkToUser(linkShareToWorkspaceDto.UsernameOrEmail, linkShareToWorkspaceDto.Password);
        if (tokenResponse.token is null)
            throw new NotFoundException("Username and password are incorrect");

        var newAddUserWorkspace = new AddUserWorkspace()
        {
            AdminId = linkShareToWorkspaceDto.AdminId,
            WorkspaceId = linkShareToWorkspaceDto.WorkspaceId,
            AppUserId = tokenResponse.appuserid
        };

        await AddUserWorkspace(newAddUserWorkspace);
    }

    public async Task UpdateAsync(UpdateWorkspaceDto updateWorkspaceDto)
    {
        var byGlobalAdmin = await _userManager.FindByIdAsync(updateWorkspaceDto.AppUserId);

        var adminRol = await _userManager.GetRolesAsync(byGlobalAdmin);

        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString())
            throw new PermisionException("No Access");

        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == updateWorkspaceDto.WorkspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Not Found");

        //_mapper.Map(updateWorkspaceDto, worksPace);
        worksPace.Title = updateWorkspaceDto.Title;
        worksPace.Description = updateWorkspaceDto.Description;
        _appDbContext.Workspaces.Update(worksPace);
        await _appDbContext.SaveChangesAsync();
    }
}
