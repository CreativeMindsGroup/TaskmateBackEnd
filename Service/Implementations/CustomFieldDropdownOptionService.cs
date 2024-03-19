using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldDropdownOption;
using TaskMate.Entities;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldDropdownOptionService : ICustomFieldDropdownOptionService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CustomFieldDropdownOptionService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task CreateAsync(List<CreateCustomFieldDropdownOption> createCustomFieldDropdownOption, Guid CustomFieldId)
    {
        foreach (var dropdownOptionDto in createCustomFieldDropdownOption)
        {
            var dropdownOption = new CustomFieldDropdownOptions
            {
                CustomFieldsId = CustomFieldId,
                Option = dropdownOptionDto.Option,
                Color = dropdownOptionDto.Color,
            };

            await _appDbContext.CustomFieldDropdownOptions.AddAsync(dropdownOption);
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid CustomFieldDropdownOptionId)
    {
        var CustomFieldDropdownOption = await _appDbContext.CustomFieldDropdownOptions
                                                .FirstOrDefaultAsync(x=>x.Id==CustomFieldDropdownOptionId);
        if (CustomFieldDropdownOption is null) throw new DirectoryNotFoundException();
        
        _appDbContext.CustomFieldDropdownOptions.Remove(CustomFieldDropdownOption);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(UpdateCustomFieldDropdownOption updateCustomFieldDropdownOption)
    {
        var CustomFieldDropdownOption = await _appDbContext.CustomFieldDropdownOptions
                                              .FirstOrDefaultAsync(x => x.Id == updateCustomFieldDropdownOption.Id);
        if (CustomFieldDropdownOption is null) throw new DirectoryNotFoundException();

        CustomFieldDropdownOption.Option = updateCustomFieldDropdownOption.Option;
        CustomFieldDropdownOption.Color = updateCustomFieldDropdownOption.Color;

        _appDbContext.CustomFieldDropdownOptions.Update(CustomFieldDropdownOption);
        await _appDbContext.SaveChangesAsync();
    }
}
