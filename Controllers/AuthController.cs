using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Auth;
using TaskMate.Helper.Auth;
using TaskMate.Helper;
using TaskMate.Service.Abstraction;
using TaskMate.Context;
using Microsoft.EntityFrameworkCore;

namespace TaskMate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly AppDbContext _context;
    public AuthController(IAuthService authService, IEmailService emailService, AppDbContext context)
    {
        _authService = authService;
        _emailService = emailService;
        _context = context;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var responseToken = await _authService.Login(loginDTO);
        return Ok(responseToken);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        ArgumentNullException.ThrowIfNull(registerDTO, ExceptionResponseMessages.ParametrNotFoundMessage);

        SignUpResponse response = await _authService.Register(registerDTO)
                ?? throw new SystemException(ExceptionResponseMessages.NotFoundMessage);

        if (response.Errors != null)
        {
            if (response.Errors.Count > 0)
            {
                return BadRequest(response.Errors);
            }
        }
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RefreshToken([FromQuery] string ReRefreshtoken)
    {
        var response = await _authService.ValidRefleshToken(ReRefreshtoken);
        return Ok(response);
    }
    [HttpPost("GetUsersByItsMail")]
    public async Task<IActionResult> GetUsersByItsMail([FromQuery] string mail)
    {
        if (string.IsNullOrWhiteSpace(mail))
        {
            return BadRequest("Email parameter is required.");
        }

        try
        {
            var users = await _context.AppUsers
                .Where(u => EF.Functions.Like(u.Email, $"%{mail}%"))
                .Take(8)
                .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found with the specified email.");
            }

            return Ok(users);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving users. Details: " + ex.Message);
        }
    }
}