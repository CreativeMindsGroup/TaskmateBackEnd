using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFileds;
using TaskMate.DTOs.DropDownOptionsDTO;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations
{
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
                return roleEnum == Role.GlobalAdmin || roleEnum == Role.Admin;
            }
            else
            {
                throw new ArgumentException("The role value in the database is undefined in the Role enum.");
            }
        }

        public async Task CreateChecklistAsync(CreateCheckboxCustomFieldDto dto)
        {
            var result = CheckUserAdminRoleInWorkspace(dto.UserId, dto.WorkspaceId);
            if (await result == false)
                throw new NotFoundException("No Access");

            var cards = await GetAllCardsByBoardIdAsync(dto.BoardId);

            foreach (var card in cards)
            {
                var customField = card.CustomFields ?? new CustomFields
                {
                    CardId = card.Id
                };

                if (card.CustomFields == null)
                {
                    _appDbContext.CustomFields.Add(customField);
                    await _appDbContext.SaveChangesAsync();
                }

                CustomFieldsCheckbox checkbox = new()
                {
                    Check = dto.Check,
                    CustomFieldsId = customField.Id,
                    Title = dto.Title
                };

                _appDbContext.CustomFieldsCheckboxes.Add(checkbox);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateNumberAsync(CustomFieldNumberDto dto)
        {
            var result = CheckUserAdminRoleInWorkspace(dto.UserId, dto.WorkspaceId);
            if (await result == false)
                throw new NotFoundException("No Access");

            var cards = await GetAllCardsByBoardIdAsync(dto.BoardId);

            foreach (var card in cards)
            {
                var customField = card.CustomFields ?? new CustomFields
                {
                    CardId = card.Id
                };

                if (card.CustomFields == null)
                {
                    _appDbContext.CustomFields.Add(customField);
                    await _appDbContext.SaveChangesAsync();
                }

                CustomFieldsNumber number = new()
                {
                    Number = dto.Number,
                    CustomFieldsId = customField.Id,
                    Title = dto.Title
                };

                _appDbContext.CustomFieldsNumbers.Add(number);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<GetCustomFieldDto> GetCustomFieldsAsync(Guid cardId)
        {
            var customFields = await _appDbContext.CustomFields
                .Include(cf => cf.Checkbox)
                .Include(cf => cf.Number)
                .Include(cf => cf.DropDown) // Include DropDown
                    .ThenInclude(dd => dd.DropDownOptions) // ThenInclude DropDownOptions
                    .Where(x=>x.CardId == cardId)   
                .FirstOrDefaultAsync();

            var customFieldDto = _mapper.Map<GetCustomFieldDto>(customFields);

            return customFieldDto;
        }
        public async Task UpdateCustomField(string value, Guid Id)
        {
            var CustomField = await _appDbContext.CustomFieldsNumbers.FirstOrDefaultAsync(cf => cf.Id == Id);
            if (CustomField is null)
            {
                throw new NotFoundException("Custom fields not found for the specified card.");
            }
            CustomField.Number = value;
            _appDbContext.CustomFieldsNumbers.Update(CustomField);
            await _appDbContext.SaveChangesAsync();
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

        private async Task<List<Card>> GetAllCardsByBoardIdAsync(Guid boardId)
        {
            var cards = await _appDbContext.Cards
                 .Include(c => c.CardList)
                 .Include(c => c.CustomFields)
                 .Where(c => c.CardList.BoardsId == boardId)
                 .OrderBy(c => c.Order)
                 .ToListAsync();

            return cards;
        }

      


        public async Task RemoveDropDown(Guid DropdownId)
        {
            var Result = await _appDbContext.DropDownOptions.FirstOrDefaultAsync(x => x.Id == DropdownId);
            if (Result is null)
            {
                throw new Exception();
            }
            _appDbContext.DropDownOptions.Remove(Result);
            await _appDbContext.SaveChangesAsync();


        }
        public async Task CreateDropdown(CreateDropdownDTO dto)
        {
            var result = await CheckUserAdminRoleInWorkspace(dto.UserId.ToString(), dto.WorkspaceId);
            if (!result)
                throw new NotFoundException("No Access");

            var cards = await GetAllCardsByBoardIdAsync(dto.BoardId);

            foreach (var card in cards)
            {
                var customField = card.CustomFields ?? new CustomFields
                {
                    CardId = card.Id
                };

                if (card.CustomFields == null)
                {
                    _appDbContext.CustomFields.Add(customField);
                    await _appDbContext.SaveChangesAsync();
                }

                // Map DTO to DropDown entity
                var dropDown = new DropDown
                {
                    Title = dto.Title,
                    CustomFieldsId = customField.Id,
                    DropDownOptions = dto.DropDownOptions?.Select(opt => new DropDownOptions
                    {
                        OptionName = opt.OptionName,
                        Color = opt.Color
                    }).ToList()
                };

                _appDbContext.DropDowns.Add(dropDown);
            }

            await _appDbContext.SaveChangesAsync();
        }


        public async Task SetOptionToDropdown(Guid dropdownId, Guid dropdownOptionId)
        {
            var dropdownOption = await _appDbContext.DropDownOptions.FirstOrDefaultAsync(x => x.Id == dropdownOptionId);
            if (dropdownOption == null)
            {
                throw new ArgumentException("Dropdown option not found", nameof(dropdownOption));
            }

            var dropdown = await _appDbContext.DropDowns.FirstOrDefaultAsync(x => x.Id == dropdownId);
            if (dropdown == null)
            {
                throw new ArgumentException("Dropdown not found", nameof(dropdown));
            }

            dropdown.SelectedId = dropdownOption.Id;
            dropdown.Color = dropdownOption.Color;
            dropdown.OptionName = dropdownOption.OptionName;
            await _appDbContext.SaveChangesAsync();


        }


    }
}
