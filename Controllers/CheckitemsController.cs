using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Checkitem;
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
}
