using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomFieldDate;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldDateService : ICustomFieldDateService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CustomFieldDateService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task CreateAsync(Guid CustomFieldId)
    {
        var newCustomFieldDate = new CustomFieldsDate();
        newCustomFieldDate.CustomFieldsId = CustomFieldId;
        await _appDbContext.CustomFieldsDates.AddAsync(newCustomFieldDate);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Remove(Guid CustomFieldId)
    {
        var CustomFieldDate = await _appDbContext.CustomFieldsDates
                        .FirstOrDefaultAsync(x => x.CustomFieldsId == CustomFieldId);
        if (CustomFieldDate is null) throw new NotFoundException("Not Found Custom Field");

        CustomFieldDate.DateTime = null;

        _appDbContext.CustomFieldsDates.Update(CustomFieldDate);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(UpdateCustomFieldDateDto updateCustomFieldDateDto)
    {
        var CustomFieldDate = await _appDbContext.CustomFieldsDates
                          .FirstOrDefaultAsync(x => x.Id == updateCustomFieldDateDto.Id);
        if (CustomFieldDate is null) throw new NotFoundException("Not Found Custom Field Date");

        CustomFieldDate.DateTime = updateCustomFieldDateDto.DateTime;

        _appDbContext.CustomFieldsDates.Update(CustomFieldDate);
        await _appDbContext.SaveChangesAsync();
    }
}
