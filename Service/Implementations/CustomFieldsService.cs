using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFileds;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class CustomFieldsService : ICustomFieldsService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CustomFieldsService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<bool> CheckUserAdminRoleInWorkspace(string userId, Guid workspaceId)
    {
        var user = await _appDbContext.WorkspaceUsers
                    .FirstOrDefaultAsync(wu => wu.WorkspaceId == workspaceId && wu.AppUserId == userId);

        if (user == null)
        {
            throw new NotFoundException("User not found in workspace!");
        }

        if (Enum.TryParse<Role>(user.Role, true, out var roleEnum))
        {
            if (roleEnum == Role.GlobalAdmin || roleEnum == Role.Admin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            throw new ArgumentException("The role value in the database is undefined in the Role enum.");
        }
    }
    public async Task CreateChecklistAsync(CreateCheckboxCustomFieldDto Dto)
    {
        var Result = CheckUserAdminRoleInWorkspace(Dto.UserId, Dto.WorkspaceId);
        if (await Result == false)
            throw new NotFoundException("No Access");
        var card = await _appDbContext.Cards
                                .Include(x => x.CustomFields)
                                .FirstOrDefaultAsync(x => x.Id == Dto.CardId);

        if (card == null)
            throw new NotFoundException("Card not found");

        if (card.CustomFields is null)
        {
            CustomFields customField = new()
            {
                CardId = Dto.CardId,
            };
            _appDbContext.CustomFields.Add(customField);
            await _appDbContext.SaveChangesAsync();
        }
        CustomFieldsCheckbox Checkboxe = new()
        {
            Check = Dto.Check,
            CustomFieldsId = card.CustomFields.Id,
            Title = Dto.Title,
        };
        _appDbContext.CustomFieldsCheckboxes.Add(Checkboxe);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task CreateNumberAsync(CustomFieldNumberDto Dto)
    {
        var result = CheckUserAdminRoleInWorkspace(Dto.UserId, Dto.WorkspaceId);
        if (await result == false)
            throw new NotFoundException("No Access");

        var card = await _appDbContext.Cards
                                      .Include(x => x.CustomFields)
                                      .FirstOrDefaultAsync(x => x.Id == Dto.CardId);

        if (card == null)
            throw new NotFoundException("Card not found");

        if (card.CustomFields is null)
        {
            CustomFields customField = new()
            {
                CardId = Dto.CardId,
            };
            _appDbContext.CustomFields.Add(customField);
            await _appDbContext.SaveChangesAsync();
        }
        CustomFieldsNumber number = new()
        {
            Number = Dto.Number,
            CustomFieldsId = card.CustomFields.Id,
            Title = Dto.Title,
        };
        _appDbContext.CustomFieldsNumbers.Add(number);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task<GetCustomFieldDto> GetCustomFieldsAsync(Guid cardId)
    {
        var customFields = await _appDbContext.CustomFields
            .Include(cf => cf.Checkbox)
            .Include(cf => cf.Number)
            .FirstOrDefaultAsync(cf => cf.CardId == cardId);

        if (customFields == null)
        {
            throw new NotFoundException("Custom fields not found for the specified card.");
        }

        var customFieldDto = _mapper.Map<GetCustomFieldDto>(customFields);

        if (customFields.Checkbox != null)
        {
            customFieldDto.CheckboxDto = _mapper.Map<List<GetCustomFieldCheckboxDto>>(customFields.Checkbox);
        }

        if (customFields.Number != null)
        {
            customFieldDto.NumberDto = _mapper.Map<List<GetCustomFiledNumber>>(customFields.Number);
        }

        return customFieldDto;
    }
    public async Task RemoveCustomField(RemoveCustomFieldDTO dto)
    {
        if (!await CheckUserAdminRoleInWorkspace(dto.UserId, dto.WorkspaceId))
        {
            throw new NotFoundException("No Access");
        }

        var customCheckboxField = await _appDbContext.CustomFieldsCheckboxes
            .FirstOrDefaultAsync(cf => cf.Id == dto.FieldId);

        if (customCheckboxField != null)
        {
            _appDbContext.CustomFieldsCheckboxes.Remove(customCheckboxField);
        }
        else
        {
            var customNumberField = await _appDbContext.CustomFieldsNumbers
                .FirstOrDefaultAsync(cf => cf.Id == dto.FieldId);

            if (customNumberField == null)
            {
                throw new NotFoundException("Custom field not found.");
            }

            _appDbContext.CustomFieldsNumbers.Remove(customNumberField);
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateChecklist(bool value, Guid id)
    {
        var checkbox = await _appDbContext.CustomFieldsCheckboxes.FirstOrDefaultAsync(cf => cf.Id == id);
        if (checkbox == null)
        {
            throw new NotFoundException("Checkbox not found!");
        }
        checkbox.Check = value;
        await _appDbContext.SaveChangesAsync();
    }
    public Task RemoveAsync(Guid CustomFieldId)
    {
        throw new NotImplementedException();
    }
}
