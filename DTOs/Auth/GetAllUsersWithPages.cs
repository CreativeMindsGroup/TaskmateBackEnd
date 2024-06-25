namespace TaskMate.DTOs.Auth
{
    public class GetAllUsersWithPages
    {
        public Guid WorkspaceId { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
