using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.CustomFieldDate;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldDatesController : ControllerBase
{
    private readonly ICustomFieldDateService _customFieldDateService;

    public CustomFieldDatesController(ICustomFieldDateService customFieldDateService)
       => _customFieldDateService = customFieldDateService;

    [HttpPut]
    public async Task<IActionResult> UpdateCustomFieldCheckbox([FromForm] UpdateCustomFieldDateDto updateCustomFieldDateDto)
    {
        await _customFieldDateService.Update(updateCustomFieldDateDto);
        return Ok();
    }


    [HttpDelete]
    public async Task<IActionResult> Remove(Guid CustomFieldId)
    {
        await _customFieldDateService.Remove(CustomFieldId);
        return Ok();
    }
}
