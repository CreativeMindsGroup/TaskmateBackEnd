using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFileds;
using TaskMate.DTOs.DropDownOptionsDTO;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldsController : ControllerBase
{
    private readonly ICustomFieldsService _customFieldsService;
    public CustomFieldsController(ICustomFieldsService customFieldsService)
        => _customFieldsService = customFieldsService;

    [HttpGet]
    public async Task<GetCustomFieldDto> GetCustomFieldsAsync(Guid cardId)
    {
        var customFields = await _customFieldsService.GetCustomFieldsAsync(cardId);
        return customFields;
    }
    [HttpPost("addChecklist")]
    public async Task<IActionResult> CreateChecklistAsync(CreateCheckboxCustomFieldDto Dto)
    {
        await _customFieldsService.CreateChecklistAsync(Dto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpPost("CreateNumber")]
    public async Task<IActionResult> CreateNumberAsync(CustomFieldNumberDto Dto)
    {
        await _customFieldsService.CreateNumberAsync(Dto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpDelete("RemoveCustomField")]
    public async Task<IActionResult> RemoveCustomField(RemoveCustomFieldDTO dto)
    {
        await _customFieldsService.RemoveCustomField(dto);
        return StatusCode((int)HttpStatusCode.OK);
    }
    [HttpPut("UpdateChecklist")]
    public async Task<IActionResult> UpdateChecklist(bool value, Guid id, string UserId, Guid WorkspaceId)
    {
        await _customFieldsService.UpdateChecklist(value, id, UserId,WorkspaceId);
        return StatusCode((int)HttpStatusCode.OK);
    }
    [HttpPut("UpdateNumberField")]
    public async Task<IActionResult> UpdateCustomField(string value, Guid Id, string UserId, Guid WorkspaceId)
    {
        await _customFieldsService.UpdateCustomField(value, Id, UserId, WorkspaceId);
        return StatusCode((int)HttpStatusCode.Accepted);
    }

    [HttpPost("RemoveDropDown")]
    public async Task<IActionResult> RemoveDropDown([FromBody] RemoveDropDownDto Dto)
    {
        await _customFieldsService.RemoveDropDown(Dto);
        return StatusCode((int)HttpStatusCode.OK);
    }

    [HttpPost("CreateDropdown")]
    public async Task<IActionResult> CreateDropdown(CreateDropdownDTO dto)
    {
        await _customFieldsService.CreateDropdown(dto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpPost("SetOptionToDropdown")]
    public async Task<IActionResult> SetOptionToDropdown(Guid dropDownId, Guid dropdownOptionId, string UserId, Guid WorkspaceId)
    {
        await _customFieldsService.SetOptionToDropdown(dropDownId, dropdownOptionId, UserId, WorkspaceId);
        return StatusCode((int)HttpStatusCode.OK);
    }
}
