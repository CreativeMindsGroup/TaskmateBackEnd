using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.CustomFieldText;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldTextsController : ControllerBase
{
    private readonly ICustomFieldTextService _customFieldTextService;

    public CustomFieldTextsController(ICustomFieldTextService customFieldTextService)
       => _customFieldTextService = customFieldTextService;

    [HttpPut]
    public async Task<IActionResult> UpdateCustomFieldText([FromForm] UpdateCustomFieldTextDto updateCustomFieldTextDto)
    {
        await _customFieldTextService.Update(updateCustomFieldTextDto);
        return Ok();
    }
}
