namespace TaskMate.DTOs.CardMembers
{
    public class AddMemberToCardDto
    {
        public Guid MemberId { get; set; }
        public Guid CardId { get; set; }
        public Guid AdminId { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
