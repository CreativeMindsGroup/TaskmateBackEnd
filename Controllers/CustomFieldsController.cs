using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Context;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFileds;
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
    public async Task<GetCustomFieldDto>GetCustomFieldsAsync(Guid cardId)
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
    public async Task<IActionResult> UpdateChecklist(bool value, Guid id)
    {
        await _customFieldsService.UpdateChecklist(value, id);
        return StatusCode((int)HttpStatusCode.OK);  
    }
}
