using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.CustomField;
using TaskMate.Helper.Enum.CustomFields;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldsController : ControllerBase
{
    private readonly ICustomFieldsService _customFieldsService;
    public CustomFieldsController(ICustomFieldsService customFieldsService)
        => _customFieldsService = customFieldsService;


    [HttpGet]
    public async Task<IActionResult> GetAll(Guid CardId)
    {
        var customFields = await _customFieldsService.GetCardInCustomFieldAsync(CardId);
        return Ok(customFields);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomField([FromBody] CreateCustomFieldDto createCustomFieldDto)
    {
        await _customFieldsService.CreateAsync(createCustomFieldDto);
        return StatusCode((int)HttpStatusCode.Created);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateCustomField([FromForm] UpdateCustomFieldDto updateCustomFieldDto)
    {
        await _customFieldsService.Update(updateCustomFieldDto);
        return Ok();
    }


    [HttpDelete]
    public async Task<IActionResult> Remove(Guid CustomFieldId)
    {
        await _customFieldsService.RemoveAsync(CustomFieldId);
        return Ok();
    }
}
