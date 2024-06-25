using System.ComponentModel.DataAnnotations;

namespace TaskMate.DTOs.Workspace
{
    public class RemoveUserFromWorksapceDto
    {

            [Required]
            public string UserId { get; set; }
            [Required]
            public Guid WorkspaceId { get; set; }
            [Required]
            public string AdminId { get; set; }
    }

}
