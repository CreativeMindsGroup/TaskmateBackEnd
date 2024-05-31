using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.Label;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelsController(ILabelService labelService)
        => _labelService = labelService;


    [HttpPost]
    public async Task<IActionResult> CreateLabel([FromForm] CreateLabelDto createLabelDto)
    {
        await _labelService.CreateAsync(createLabelDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

}
