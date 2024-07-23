namespace TaskMate.DTOs.Users
{
    public record CreateUserDto( string Email, string Password, string UserRole, Guid AdminId);
}
