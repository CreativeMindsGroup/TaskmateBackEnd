namespace TaskMate.DTOs.Users
{
    public record CreateUserDto( string? Fullname, string Username, string Email, string Password, string UserRole, Guid AdminId);
}
