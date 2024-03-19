using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.CustomFieldDropdownOption;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldDropdownOptionsController : ControllerBase
{
    private readonly ICustomFieldDropdownOptionService _customFieldDropdownOptionService;
    public CustomFieldDropdownOptionsController(ICustomFieldDropdownOptionService customFieldDropdownOptionService)
        => _customFieldDropdownOptionService = customFieldDropdownOptionService;

    [HttpPut]
    public async Task<IActionResult> UpdateCustomFieldDropdownOption([FromForm] UpdateCustomFieldDropdownOption updateCustomFieldDropdownOption)
    {
        await _customFieldDropdownOptionService.Update(updateCustomFieldDropdownOption);
        return Ok();
    }


    [HttpDelete]
    public async Task<IActionResult> Remove(Guid CustomFieldDropdownOptionId)
    {
        await _customFieldDropdownOptionService.RemoveAsync(CustomFieldDropdownOptionId);
        return Ok();
    }
}
