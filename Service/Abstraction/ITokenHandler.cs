using TaskMate.DTOs.Auth;
using TaskMate.Entities;

namespace TaskMate.Service.Abstraction;

public interface ITokenHandler
{
    public Task<TokenResponseDTO> CreateAccessToken(int minutes, int refreshTokenMinutes, AppUser appUser);
}
