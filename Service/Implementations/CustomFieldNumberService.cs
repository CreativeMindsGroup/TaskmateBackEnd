using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldNumberService : ICustomFieldNumberService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CustomFieldNumberService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task CreateAsync(Guid CustomFieldId)
    {
        var newCustomFieldNumber = new CustomFieldsNumber();
        newCustomFieldNumber.CustomFieldsId = CustomFieldId;
        await _appDbContext.CustomFieldsNumbers.AddAsync(newCustomFieldNumber);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(UpdateCustomFieldsNumberDto updateCustomFieldsNumberDto)
    {
        var CustomFieldNumber = await _appDbContext.CustomFieldsNumbers
                           .FirstOrDefaultAsync(x => x.Id == updateCustomFieldsNumberDto.Id);
        if (CustomFieldNumber is null) throw new NotFoundException("Not Found Custom Field Number");


        CustomFieldNumber.Number = updateCustomFieldsNumberDto.Number;

        _appDbContext.CustomFieldsNumbers.Update(CustomFieldNumber);
        await _appDbContext.SaveChangesAsync();
    }
}
