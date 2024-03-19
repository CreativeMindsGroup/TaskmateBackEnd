using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldsNumbersController : ControllerBase
{
    private readonly ICustomFieldNumberService _customFieldNumberService;

    public CustomFieldsNumbersController(ICustomFieldNumberService customFieldNumberService)
       => _customFieldNumberService = customFieldNumberService;

    [HttpPut]
    public async Task<IActionResult> UpdateCustomFieldNumber([FromForm] UpdateCustomFieldsNumberDto updateCustomFieldsNumberDto)
    {
        await _customFieldNumberService.Update(updateCustomFieldsNumberDto);
        return Ok();
    }
}
