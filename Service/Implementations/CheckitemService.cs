using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Checkitem;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

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
    public async Task CreateAsync(CreateCheckitemDto createCheckitemDto)
    {
        if (await _appDbContext.Checklists.FirstOrDefaultAsync(x => x.Id == createCheckitemDto.ChecklistId) is null)
            throw new NotFoundException("Not Found");

        var newCheckItem = _mapper.Map<Checkitem>(createCheckitemDto);

        await _appDbContext.Checkitems.AddAsync(newCheckItem);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<GetCheckItemCountDto> GetChecklistInItemCount(Guid ChecklistId)
    {
        return new GetCheckItemCountDto()
        {
            True = _appDbContext.Checkitems.Where(x => x.ChecklistId == ChecklistId && x.Check == true).Count(),
            False = _appDbContext.Checkitems.Where(x => x.ChecklistId == ChecklistId && x.Check == false).Count()
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
}
