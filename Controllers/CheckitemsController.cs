using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Checkitem;
using TaskMate.Exceptions;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckitemsController : ControllerBase
{
    private readonly ICheckitemService _checkitemService;
    public CheckitemsController(ICheckitemService checkitemService)
        => _checkitemService = checkitemService;

    [HttpPost]
    public async Task<IActionResult> Createcheckitem([FromForm] CreateCheckitemDto createCheckitemDto)
    {
        await _checkitemService.CreateAsync(createCheckitemDto);
        return StatusCode((int)HttpStatusCode.Created);
    }
    [HttpGet("GetChecklistItemCount")]
    public async Task<GetCheckItemCountDto> GetChecklistInItemCount(Guid CardId)
    {
        var result = await _checkitemService.GetChecklistInItemCount(CardId);
        return result;
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCheckItem([FromForm] UpdateCheckitemDto updateCheckitemDto)
    {
        await _checkitemService.UpdateAsync(updateCheckitemDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid CheckItemId)
    {
        await _checkitemService.RemoveAsync(CheckItemId);
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklistState(Guid id, [FromBody] bool state)
    {
        try
        {
            await _checkitemService.UpdateStateOfChecklist(id, state);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            // Log the exception details here
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the checklist item.");
        }
    }


}
