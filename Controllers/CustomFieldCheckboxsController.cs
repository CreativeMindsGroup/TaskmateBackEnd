using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomFieldCheckboxsController : ControllerBase
{
    private readonly ICustomFieldCheckboxService _customFieldCheckboxService;

    public CustomFieldCheckboxsController(ICustomFieldCheckboxService customFieldCheckboxService)
       => _customFieldCheckboxService = customFieldCheckboxService;

    [HttpPut]
    public async Task<IActionResult> UpdateCustomFieldCheckbox([FromForm] UpdateCustomFieldCheckboxDto updateBoardsDto)
    {
        await _customFieldCheckboxService.Update(updateBoardsDto);
        return Ok();
    }

}
