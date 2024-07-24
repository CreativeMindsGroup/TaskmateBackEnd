using Microsoft.AspNetCore.Identity;

namespace TaskMate.Entities;

public class AppUser : IdentityUser
{
    public bool isActive { get; set; }
    public DateTime RefreshTokenExpration { get; set; }
    public DateTime? LastActive { get; set; }
    public string? RefreshToken { get; set; }
    public List<WorkspaceUser>? WorkspaceUserss { get; set; }
    public List<UserBoards>? UserBoards { get; set; }
    public List<Comment>? Comments { get; set; }
    public ICollection<AppUsersCards>? AppUsersCards { get; set; }

}