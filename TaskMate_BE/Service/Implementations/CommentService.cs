using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Comment;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CommentService : ICommentService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWorkspaceService _workspaceService;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public CommentService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper, IWorkspaceService workspaceService, IAuthService authService)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _mapper = mapper;
        _workspaceService = workspaceService;
        _authService = authService;
    }

    public async Task CreateAsync(CreateCommentDto createCommentDto)
    {
        var newComment = _mapper.Map<Comment>(createCommentDto);

        await _appDbContext.Comments.AddAsync(newComment);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<GetCommentDto>> GetByCardComments(Guid CardId)
    {
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == CardId);
        if (card is null)
            throw new NotFoundException("Not Found Card");
       
        var Comments = await _appDbContext.Comments.Where(x => x.CardId == CardId).ToListAsync();
        return _mapper.Map<List<GetCommentDto>>(Comments);
    }

    public async Task RemoveAsync(string AppUserId, Guid CommentId)
    {
        var comment = await _appDbContext.Comments.FirstOrDefaultAsync(x => x.AppUserId == AppUserId && x.Id == CommentId);
        bool isRemove = false;
        if (comment is not null)
        {
            _appDbContext.Comments.Remove(comment);
            await _appDbContext.SaveChangesAsync();
            isRemove = true;
        }
        if (isRemove is false)
        {
            var adminAccesComment = await _appDbContext.Comments.FirstOrDefaultAsync(x =>x.Id == CommentId);

            var byAdmin = await _userManager.FindByIdAsync(AppUserId);

            var adminRol = await _userManager.GetRolesAsync(byAdmin);

            if (adminRol.FirstOrDefault().ToString() == Role.GlobalAdmin.ToString() ||
               adminRol.FirstOrDefault().ToString() == Role.Admin.ToString())
            {
                _appDbContext.Comments.Remove(adminAccesComment);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new PermisionException("No Access");
            }
        }
    }

    public async Task UpdateAsync(UpdateCommentDto updateCommentDto)
    {
        var comment = await _appDbContext.Comments.FirstOrDefaultAsync(x => x.Id == updateCommentDto.Id && x.AppUserId == updateCommentDto.AppUserId);
        if (comment is null)
            throw new NotFoundException("Not Found Comment");

        _mapper.Map(updateCommentDto, comment);

        _appDbContext.Comments.Update(comment);
        await _appDbContext.SaveChangesAsync();
    }
}
