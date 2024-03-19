namespace TaskMate.DTOs.Auth;
public record RegisterDTO(string? Fullname, string Username, string Email, string password,string userRole);