using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.MapperProfile.CustomField;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldCheckboxService : ICustomFieldCheckboxService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CustomFieldCheckboxService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task CreateAsync(Guid CustomFieldId)
    {
        var newCustomFieldCheckbox = new CustomFieldsCheckbox();
        newCustomFieldCheckbox.CustomFieldsId = CustomFieldId;
        await _appDbContext.CustomFieldsCheckboxes.AddAsync(newCustomFieldCheckbox);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(UpdateCustomFieldCheckboxDto updateCustomFieldCheckboxDto)
    {
        var CustomFieldCheckbox = await _appDbContext.CustomFieldsCheckboxes
                            .FirstOrDefaultAsync(x => x.Id == updateCustomFieldCheckboxDto.Id);
        if (CustomFieldCheckbox is null) throw new NotFoundException("Not Found Custom Field Checkbox");


        CustomFieldCheckbox.Check = !CustomFieldCheckbox.Check;

        _appDbContext.CustomFieldsCheckboxes.Update(CustomFieldCheckbox);
        await _appDbContext.SaveChangesAsync();
    }
}
