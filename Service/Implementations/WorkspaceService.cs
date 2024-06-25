using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMate.Context;
using TaskMate.DTOs.Auth;
using TaskMate.DTOs.Users;
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
    private readonly IConfiguration _configuration;

    public WorkspaceService(AppDbContext appDbContext,
                           UserManager<AppUser> userManager,
                           IMapper mapper,
                           IAuthService authService,
                           IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
        _authService = authService;
        _configuration = configuration;
    }


    public async Task CreateAsync(CreateWorkspaceDto createWorkspaceDto)
    {
        using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var byGlobalAdmin = await _userManager.FindByIdAsync(createWorkspaceDto.AppUserId);
                var roles = await _userManager.GetRolesAsync(byGlobalAdmin);

                if (!roles.Any(r => r == Role.GlobalAdmin.ToString()))
                {
                    throw new PermisionException("No Access");
                }

                var newWorkspace = _mapper.Map<Workspace>(createWorkspaceDto);
                await _appDbContext.Workspaces.AddAsync(newWorkspace);
                await _appDbContext.SaveChangesAsync();

                // Optionally add the creating user as a member/admin of the workspace

                var workspaceUser = new WorkspaceUser
                {
                    AppUserId = createWorkspaceDto.AppUserId,
                    WorkspaceId = newWorkspace.Id,
                    Role = Role.GlobalAdmin.ToString()
                };
                await _appDbContext.WorkspaceUsers.AddAsync(workspaceUser);
                await _appDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw the exception to handle it further up the stack
            }
        }
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
    public async Task<string> GetUserRole(string userId, Guid workspaceId)
    {
        var user = await _appDbContext.WorkspaceUsers
                   .FirstOrDefaultAsync(wu => wu.WorkspaceId == workspaceId && wu.AppUserId == userId);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {userId} not found.");
        }

        if (Enum.TryParse<Role>(user.Role, true, out var roleEnum)) 
        {
            return roleEnum.ToString(); 
        }
        else
        {
            throw new ArgumentException("The role value in the database is undefined in the Role enum.");
        }
    }
    public async Task<List<GetUserDto>> GetAllUsersInWorkspace(Guid workspaceId, int page, int pageSize)
    {
        if (workspaceId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(workspaceId), "Workspace ID is null or empty.");
        }

        // Calculate the number of records to skip
        int skip = (page - 1) * pageSize;
        var users = await _appDbContext.WorkspaceUsers
                                       .Where(wu => wu.WorkspaceId == workspaceId)
                                       .Include(wu => wu.AppUser)
                                       .OrderBy(wu => wu.AppUserId) // Make sure to order by some property
                                       .Skip(skip)
                                       .Take(pageSize)
                                       .Select(wu => wu.AppUser)
                                       .ToListAsync();
        var userList = new List<GetUserDto>();

        foreach (var user in users)
        {
            var roles = await GetUserRole(user.Id,workspaceId);
            var result = _mapper.Map<GetUserDto>(user);
            result.Role = roles ?? "";
            userList.Add(result);
        }
        return userList;
    }
    public async Task<RoleCountsDto> GetAllUsersInWorkspaceCount(Guid workspaceId)
    {
        if (workspaceId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(workspaceId), "Workspace ID is null or empty.");
        }

        var users = await _appDbContext.WorkspaceUsers
                                       .Where(wu => wu.WorkspaceId == workspaceId)
                                       .Include(wu => wu.AppUser)
                                       .Select(wu => wu.AppUser)
                                       .ToListAsync();

        var roleCounts = new RoleCountsDto();

        foreach (var user in users)
        {
            var roles = await GetUserRole(user.Id,workspaceId);
            if (roles.Contains("Admin"))
            {
                roleCounts.AdminCount++;
            }
            if (roles.Contains("GlobalAdmin"))
            {
                roleCounts.AdminCount++;
            }
            else if (roles.Contains("Guest"))
            {
                roleCounts.GuestCount++;
            }
            roleCounts.MemberCount++;
        }

        return roleCounts;
    }

    public async Task<GetWorkspaceDto> GetByIdAsync(Guid WorspaceId)
    {
        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == WorspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Not Found");

        return _mapper.Map<GetWorkspaceDto>(worksPace);
    }
    public async Task<List<GetWorkspaceInBoardDto>> GetWorkspaceInBoards(string AppUserId)
    {
        var byAdmin = await _userManager.FindByIdAsync(AppUserId);
        var adminRol = await _userManager.GetRolesAsync(byAdmin);
        if (adminRol.FirstOrDefault().ToString() != Role.GlobalAdmin.ToString() &&
            adminRol.FirstOrDefault().ToString() != Role.Admin.ToString())
            throw new PermisionException("No Access");
        var workspacInboards = await _appDbContext.Workspaces.Include(x => x.Boards).ToListAsync();

        return _mapper.Map<List<GetWorkspaceInBoardDto>>(workspacInboards);
    }
    public async Task Remove(string AppUserId, Guid WokspaceId)
    {
        var role = GetUserRole(AppUserId, WokspaceId);

        if (await role != Role.GlobalAdmin.ToString())
            throw new PermisionException("No Access");

        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == WokspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Not Found");

        _appDbContext.Workspaces.Remove(worksPace);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task<string> GenerateTokenForWorkspaceInvite(LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        // Check if admin exists
        var adminUser = await _appDbContext.AppUsers
                        .Where(wu => wu.Id == linkShareToWorkspaceDto.AdminId).FirstOrDefaultAsync();
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["JwtSettings:Key"];
        var siteName = _configuration["Website:siteName"];
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim("AdminId", linkShareToWorkspaceDto.AdminId),
            new Claim("Email", linkShareToWorkspaceDto.Email),
            new Claim("Role", linkShareToWorkspaceDto.Role.ToString()),
            new Claim("WorkspaceId", linkShareToWorkspaceDto.WorkspaceId.ToString()),
            new Claim("Inviter", adminUser?.Email),
        }),
            Expires = DateTime.UtcNow.AddHours(24), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return $"{siteName}/workspace/invite?token={tokenHandler.WriteToken(token)}";
    }
    public async Task<IActionResult> InviteUserToWorkspace(string token,string UserEmail)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]); // Retrieve from configuration
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var adminId = principal.FindFirst("AdminId").Value;
            var email = UserEmail;
            var role = principal.FindFirst("Role").Value;
            var workspaceId = Guid.Parse(principal.FindFirst("WorkspaceId").Value);
            await AddNewUserToWorkspace(new LinkShareToWorkspaceDto
            {
                AdminId = adminId,
                Email = email,
                Role = int.Parse(role),
                WorkspaceId = workspaceId
            });

            return new OkObjectResult("User successfully added to workspace.");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult("Invalid token.");
        }
    }
    public async Task AddNewUserToWorkspace(LinkShareToWorkspaceDto linkShareToWorkspaceDto)
    {
        // Check if admin exists
        var adminUser = await _appDbContext.WorkspaceUsers
                        .Where(wu => wu.AppUserId == linkShareToWorkspaceDto.AdminId).FirstOrDefaultAsync();
        if (adminUser == null)
            throw new NotFoundException("Administrator not found!");

        // Check if workspace exists
        var workspace = await _appDbContext.Workspaces
                               .FirstOrDefaultAsync(w => w.Id == linkShareToWorkspaceDto.WorkspaceId);
        if (workspace == null)
            throw new NotFoundException("Workspace not found!");

        // Attempt to find the user by email or fallback to user ID
        var user = !string.IsNullOrEmpty(linkShareToWorkspaceDto.Email) ?
                   await _userManager.FindByEmailAsync(linkShareToWorkspaceDto.Email) :
                   await _userManager.FindByIdAsync(linkShareToWorkspaceDto.UserId);

        // If no user is found by email or user ID, throw exception
        if (user == null)
            throw new NotFoundException("User not found!");

        // Check if user is already part of the workspace
        var existingWorkspaceUser = await _appDbContext.WorkspaceUsers
                            .AnyAsync(wu => wu.WorkspaceId == linkShareToWorkspaceDto.WorkspaceId && wu.AppUserId == user.Id);
        if (existingWorkspaceUser)
            return;

        // Check if admin has sufficient permissions
        var roles = await GetUserRole(linkShareToWorkspaceDto.AdminId.ToString(),linkShareToWorkspaceDto.WorkspaceId);
        if (!roles.Contains(Role.GlobalAdmin.ToString()) && !roles.Contains(Role.Admin.ToString()))
            throw new NotFoundException("No Access");

        // Create and add new workspace user
        var newWorkspaceUser = new WorkspaceUser
        {
            AppUserId = user.Id, // Use the found user's ID
            WorkspaceId = linkShareToWorkspaceDto.WorkspaceId,
            Role = linkShareToWorkspaceDto.Role.ToString()
        };

        try
        {
            await _appDbContext.WorkspaceUsers.AddAsync(newWorkspaceUser);
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while adding user to workspace", ex);
        }
    }
    public async Task UpdateAsync(UpdateWorkspaceDto updateWorkspaceDto)
    {
        var role =GetUserRole(updateWorkspaceDto.AppUserId, updateWorkspaceDto.WorkspaceId);

        if (await role != Role.GlobalAdmin.ToString())
            throw new PermisionException("No Access");

        var worksPace = await _appDbContext.Workspaces.Where(x => x.Id == updateWorkspaceDto.WorkspaceId).FirstOrDefaultAsync();
        if (worksPace is null)
            throw new NotFoundException("Workspace not found");

        //_mapper.Map(updateWorkspaceDto, worksPace);
        if (!updateWorkspaceDto.Title.IsNullOrEmpty())
        {
            worksPace.Title = updateWorkspaceDto.Title;
        }
        if (!updateWorkspaceDto.Description.IsNullOrEmpty())
        {
            worksPace.Description = updateWorkspaceDto.Description;
        }
        _appDbContext.Workspaces.Update(worksPace);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task ChangeUserRole(UpdateUserRoleDto dto)
    {
        var adminUser = await _appDbContext.WorkspaceUsers
                       .FirstOrDefaultAsync(wu => wu.WorkspaceId == dto.WorkspaceId && wu.AppUserId == dto.AdminId);
        if (adminUser == null)
        {
            throw new NotFoundException("Admin user not found.");
        }

        if (!Enum.TryParse<Role>(adminUser.Role, out var adminRole))
        {
            throw new Exception("Failed to parse role.");
        }
        if (adminRole != Role.Admin && adminRole != Role.GlobalAdmin)
        {
            throw new NotFoundException("No Access");
        }
        if (dto.Role == 3 && adminRole != Role.GlobalAdmin)
        {
            throw new NotFoundException("No Access");
        }

        var workspaceUser = await _appDbContext.WorkspaceUsers
                        .FirstOrDefaultAsync(wu => wu.WorkspaceId == dto.WorkspaceId && wu.AppUserId == dto.UserId);

        if (workspaceUser == null)
        {
            throw new NotFoundException("User not found in this workspace!");
        }
        workspaceUser.Role = dto.Role.ToString();
        _appDbContext.WorkspaceUsers.Update(workspaceUser);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task RemoveUserFromWorkspace(RemoveUserFromWorksapceDto dto)
    {
        var adminUser = await _appDbContext.WorkspaceUsers
                       .FirstOrDefaultAsync(wu => wu.WorkspaceId == dto.WorkspaceId && wu.AppUserId == dto.AdminId);
        if (adminUser == null)
        {
            throw new NotFoundException("Admin user not found.");
        }
        if (!Enum.TryParse<Role>(adminUser.Role, out var adminRole))
        {
            throw new Exception("Failed to parse role.");
        }
        if (adminRole != Role.Admin && adminRole != Role.GlobalAdmin)
        {
            throw new NotFoundException("No Access");
        }
        var workspaceUser = await _appDbContext.WorkspaceUsers
                        .FirstOrDefaultAsync(wu => wu.WorkspaceId == dto.WorkspaceId && wu.AppUserId == dto.UserId);

        if (workspaceUser == null)
        {
            throw new NotFoundException("User not found in this workspace!");
        }
        _appDbContext.WorkspaceUsers.Remove(workspaceUser);
        await _appDbContext.SaveChangesAsync();
    }

}

