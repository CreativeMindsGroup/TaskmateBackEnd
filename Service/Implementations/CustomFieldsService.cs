using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.CustomFields;
using TaskMate.MapperProfile.CustomField;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldsService : ICustomFieldsService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly ICustomFieldCheckboxService _checkboxService;
    private readonly ICustomFieldDateService _customFieldDateService;
    private readonly ICustomFieldNumberService _customFieldNumberService;
    private readonly ICustomFieldTextService _customFieldTextService;
    private readonly ICustomFieldDropdownOptionService _customFieldDropdownOptionService;

    public CustomFieldsService(AppDbContext appDbContext,
                               IMapper mapper,
                               ICustomFieldCheckboxService checkboxService,
                               ICustomFieldDateService customFieldDateService,
                               ICustomFieldNumberService customFieldNumberService,
                               ICustomFieldTextService customFieldTextService,
                               ICustomFieldDropdownOptionService customFieldDropdownOptionService)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _checkboxService = checkboxService;
        _customFieldDateService = customFieldDateService;
        _customFieldNumberService = customFieldNumberService;
        _customFieldTextService = customFieldTextService;
        _customFieldDropdownOptionService = customFieldDropdownOptionService;
    }

    public async Task CreateAsync(CreateCustomFieldDto createCustomFieldDto)
    {
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == createCustomFieldDto.CardId);
        if (card is null) throw new NotFoundException("Not Found");

        var newCustomField = _mapper.Map<CustomFields>(createCustomFieldDto);

        await _appDbContext.CustomFields.AddAsync(newCustomField);
        await _appDbContext.SaveChangesAsync();

        if (newCustomField.Type == CustomFieldsType.Checkbox)
            await _checkboxService.CreateAsync(newCustomField.Id);
        else if (newCustomField.Type == CustomFieldsType.Date)
            await _customFieldDateService.CreateAsync(newCustomField.Id);
        else if (newCustomField.Type == CustomFieldsType.Number)
            await _customFieldNumberService.CreateAsync(newCustomField.Id);
        else if (newCustomField.Type == CustomFieldsType.Text)
            await _customFieldTextService.CreateAsync(newCustomField.Id);
        else
        {
            if (createCustomFieldDto.CreateCustomFieldDropdownOptions != null)
            {
                await _customFieldDropdownOptionService.CreateAsync(createCustomFieldDto.CreateCustomFieldDropdownOptions, newCustomField.Id);
            }
        }
    }


    public async Task<List<GetCustomFieldDto>> GetCardInCustomFieldAsync(Guid CardId)
    {
        var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == CardId);
        if (card is null) throw new NotFoundException("Not Found Card");

        var allCustomField = await _appDbContext.CustomFields
                                                .Include(x=>x.CustomFieldDropdownOptions)
                                                .Include(x=>x.CustomFieldsTexts)
                                                .Include(x=>x.CustomFieldsNumbers)
                                                .Include(x=>x.CustomFieldsDates)
                                                .Include(x=>x.CustomFieldsCheckboxes)
                                                .Where(x => x.CardId == card.Id).ToListAsync();

        var toMapper = _mapper.Map<List<GetCustomFieldDto>>(allCustomField);
        foreach (var customField in toMapper)
            if (customField.Type != CustomFieldsType.Dropdown) customField.GetCustomFieldDropdownOptions = null;
        return toMapper;
    }

    public async Task RemoveAsync(Guid CustomFieldId)
    {
        var customField = await _appDbContext.CustomFields.FirstOrDefaultAsync(x=>x.Id==CustomFieldId);
        if (customField is null) throw new NotFoundException("Not Found Custom Field");

        _appDbContext.CustomFields.Remove(customField);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(UpdateCustomFieldDto updateCustomFieldDto)
    {
        var customField = await _appDbContext.CustomFields.FirstOrDefaultAsync(x => x.Id == updateCustomFieldDto.Id);
        if (customField is null) throw new NotFoundException("Not Found Custom Field");

        customField.Title = updateCustomFieldDto.Title;
        _appDbContext.CustomFields.Update(customField);
        await _appDbContext.SaveChangesAsync();
    }
}
