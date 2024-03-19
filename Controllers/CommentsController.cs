using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMate.DTOs.Boards;
using TaskMate.DTOs.Comment;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    public CommentsController(ICommentService commentService)
        => _commentService = commentService;

    [HttpGet("{CardId:Guid}")]
    public async Task<IActionResult> GetById(Guid CardId)
    {
        var comments = await _commentService.GetByCardComments(CardId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromForm] CreateCommentDto createCommentDto)
    {
        await _commentService.CreateAsync(createCommentDto);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateComment([FromForm] UpdateCommentDto updateCommentDto)
    {
        await _commentService.UpdateAsync(updateCommentDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string AppUserId, Guid CommentId)
    {
        await _commentService.RemoveAsync(AppUserId, CommentId);
        return Ok();
    }
}
