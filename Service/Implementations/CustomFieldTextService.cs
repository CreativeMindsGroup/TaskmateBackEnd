using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFieldText;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldTextService : ICustomFieldTextService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CustomFieldTextService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task CreateAsync(Guid CustomFieldId)
    {
        var newCustomFieldText = new CustomFieldsText();
        newCustomFieldText.CustomFieldsId = CustomFieldId;
        await _appDbContext.CustomFieldsTexts.AddAsync(newCustomFieldText);
        await _appDbContext.SaveChangesAsync();
    }

  
    public async Task Update(UpdateCustomFieldTextDto updateCustomFieldTextDto)
    {
        var CustomFieldText = await _appDbContext.CustomFieldsTexts
                           .FirstOrDefaultAsync(x => x.Id == updateCustomFieldTextDto.Id);
        if (CustomFieldText is null) throw new NotFoundException("Not Found Custom Field Text");


        CustomFieldText.Text = updateCustomFieldTextDto.Text;

        _appDbContext.CustomFieldsTexts.Update(CustomFieldText);
        await _appDbContext.SaveChangesAsync();
    }
}
