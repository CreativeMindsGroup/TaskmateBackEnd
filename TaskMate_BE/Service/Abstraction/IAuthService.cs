using TaskMate.DTOs.Auth;
using TaskMate.Helper.Auth;

namespace TaskMate.Service.Abstraction;

public interface IAuthService
{
    Task<SignUpResponse> Register(RegisterDTO registerDTO);
    Task<TokenResponseDTO> Login(LoginDTO loginDTO);
    Task<TokenResponseDTO> ShareLinkToUser(string UsernameOrEmail, string Password);  //link share ucundur yalniz
    Task<TokenResponseDTO> LoginAdmin(LoginDTO loginDTO);
    Task<TokenResponseDTO> ValidRefleshToken(string refreshToken);
}
