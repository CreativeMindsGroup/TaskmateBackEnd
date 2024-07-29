using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.Users;

namespace TaskMate.DTOs.Card
{
    public class CreateCardDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid CardListId { get; set; }
        public int Order { get; set; }  // Add this line
    }
    public class UpdateCardDto
    {
        public Guid CardId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }


    public class CreateCardDueDateDto
    {
        public Guid CardId { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DueDate { get; set; }
    }
    public class UpdateCardDueDateDto
    {
        public Guid CardId { get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? isDueDateDone { get; set; }
    }
    public class GetCardDto
    {

        public int Order { get; set; }
        public Guid Id { get; set; }
        public string? CoverColor { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid CardListId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool isDueDateDone { get; set; }
        public bool isArchived { get; set; }
        public List<CardAttachmentDto>? Attachments { get; set; }
        public List<GetUserDto>? AppUsers { get; set; }
        public GetCustomFieldDto? GetCustomFieldDto { get; set; }
    }
    public class GetArchivedCardDto
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool isArchived { get; set; }
    }
    public class CardAttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    
    public class CardAddDatesDto
    {
        public Guid CardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DragAndDropCardDto
    {
        public Guid CardId { get; set; }
        public Guid CardListId { get; set; }
    }

    public class ReorderCardsDto
    {
        public List<CardOrderDto> CardOrders { get; set; }
    }

    public class CardOrderDto
    {
        public Guid CardId { get; set; }
        public int Order { get; set; }
    }
    public class FileUploadDto
    {
        public Guid CardId { get; set; }
        public string? FileName { get; set; }
        public string? UserId { get; set;}
        public Guid WorkspaceId { get; set; }
    }
}
