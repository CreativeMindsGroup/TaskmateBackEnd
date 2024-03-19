namespace TaskMate.DTOs.Users
{
    public class GetUserDto
    {
        public string? Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid Id { get; set; }

        public GetUserDto()
        {
        }
        public GetUserDto(string? fullname, string username, string email, string role, Guid id)
        {
            Fullname = fullname;
            Username = username;
            Email = email;
            Role = role;
            Id = id;
        }
    }
}
