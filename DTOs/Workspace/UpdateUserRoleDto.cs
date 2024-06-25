using System.ComponentModel.DataAnnotations;
using TaskMate.Helper.Enum.User;

namespace TaskMate.DTOs.Workspace
{
    public class UpdateUserRoleDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public Guid WorkspaceId { get; set; }
        [Required]
        public int Role { get; set; }
        [Required]
        public string AdminId { get; set; }
    }
}
