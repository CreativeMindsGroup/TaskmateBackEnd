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
    
    [HttpPost("[action]")]
    public async Task<IActionResult> CheckBoxCreateLabel([FromForm] CheckCreateLabelDto createLabelDto)
    {
        await _labelService.CheckBoxCreateAsync(createLabelDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid CardId)
    {
        var labels = await _labelService.GetLabelByCardId(CardId);
        return Ok(labels);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllLabelinBoard(Guid BoardId)
    {
        var labels = await _labelService.GetAllLabelInBoard(BoardId);
        return Ok(labels);
    }

}
