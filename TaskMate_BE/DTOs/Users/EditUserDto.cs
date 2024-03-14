namespace TaskMate.DTOs.Users
{
    public record EditUserDto(Guid UserId,string? Fullname, string Username, string Email,string Password, string UserRole,Guid AdminId);
}
