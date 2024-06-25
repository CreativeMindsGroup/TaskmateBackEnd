namespace TaskMate.DTOs.Users
{
    public class GetUserDto
    {
        public string? Username { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public Guid Id { get; set; }

    }
}
