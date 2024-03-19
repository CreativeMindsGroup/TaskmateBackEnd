using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskMate.Context;
using TaskMate.DTOs.Auth;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper;
using TaskMate.Helper.Auth;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _siginManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(UserManager<AppUser> userManager,
                       SignInManager<AppUser> siginManager,
                       RoleManager<IdentityRole> roleManager,
                       AppDbContext context,
                       IConfiguration configuration,
                       ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _siginManager = siginManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
        _tokenHandler = tokenHandler;
    }


    public async Task<TokenResponseDTO> Login(LoginDTO loginDTO)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(loginDTO.UsernameOrEmail);
        if (appUser is null)
        {
            appUser = await _userManager.FindByNameAsync(loginDTO.UsernameOrEmail);
            if (appUser is null) throw new LogInFailerException("Invalid Login!");

        }

        SignInResult signInResult = await _siginManager.CheckPasswordSignInAsync(appUser, loginDTO.password, true);
        if (!signInResult.Succeeded)
        {
            throw new LogInFailerException("Invalid Login!");
        }
        //if (!appUser.isActive)
        //{
        //    throw new UserBlockedException("User Blocked");
        //}
        var tokenRespons = await _tokenHandler.CreateAccessToken(2, 3, appUser);
        appUser.RefreshToken = tokenRespons.refreshToken;
        appUser.RefreshTokenExpration = tokenRespons.refreshTokenExpration;
        await _userManager.UpdateAsync(appUser);
        return tokenRespons;
    }

    public Task<TokenResponseDTO> LoginAdmin(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }
    public async Task<SignUpResponse> Register(RegisterDTO registerDTO)
    {
        AppUser appUser = new AppUser()
        {
            Fullname = registerDTO.Fullname,
            UserName = registerDTO.Username,
            Email = registerDTO.Email,
            isActive = false,
        };
        IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerDTO.password);
        if (!identityResult.Succeeded)
        {
            StringBuilder errorMessage = new();
            foreach (var error in identityResult.Errors)
            {
                errorMessage.AppendLine(error.Description);
            }
            throw new RegistrationException(errorMessage.ToString());
        }

        var result = await _userManager.AddToRoleAsync(appUser, Role.Member.ToString());
        if (!result.Succeeded)
        {
            return new SignUpResponse
            {
                StatusMessage = ExceptionResponseMessages.UserFailedMessage,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        return new SignUpResponse
        {
            Errors = null,
            StatusMessage = ExceptionResponseMessages.UserSuccesMessage,
            UserEmail = appUser.Email
        };
    }

    public async Task<TokenResponseDTO> ShareLinkToUser(string UsernameOrEmail, string Password)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(UsernameOrEmail);
        if (appUser is null)
        {
            appUser = await _userManager.FindByNameAsync(UsernameOrEmail);
            if (appUser is null) throw new LogInFailerException("Invalid Login!");

        }

        SignInResult signInResult = await _siginManager.CheckPasswordSignInAsync(appUser, Password, true);
        if (!signInResult.Succeeded)
        {
            throw new LogInFailerException("Invalid Login!");
        }

        var tokenRespons = await _tokenHandler.CreateAccessToken(2, 3, appUser);
        appUser.RefreshToken = tokenRespons.refreshToken;
        appUser.RefreshTokenExpration = tokenRespons.refreshTokenExpration;
        await _userManager.UpdateAsync(appUser);
        return tokenRespons;
    }

    public async Task<TokenResponseDTO> ValidRefleshToken(string refreshToken)
    {
        if (refreshToken is null)
        {
            throw new ArgumentNullException("Refresh token does not exist");
        }
        var ByUser = await _context.Users.Where(a => a.RefreshToken == refreshToken).FirstOrDefaultAsync();
        if (ByUser is null)
        {
            throw new NotFoundException("User does Not Exist");
        }
        if (ByUser.RefreshTokenExpration < DateTime.UtcNow)
        {
            throw new ArgumentNullException("Refresh token does not exist");
        }

        var tokenResponse = await _tokenHandler.CreateAccessToken(2, 3, ByUser);
        ByUser.RefreshToken = tokenResponse.refreshToken;
        ByUser.RefreshTokenExpration = tokenResponse.refreshTokenExpration;
        await _userManager.UpdateAsync(ByUser);
        return tokenResponse;
    }
}