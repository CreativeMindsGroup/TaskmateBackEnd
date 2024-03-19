using TaskMate.DTOs.Comment;

namespace TaskMate.Service.Abstraction;

public interface ICommentService
{
    Task CreateAsync(CreateCommentDto createCommentDto);
    Task UpdateAsync(UpdateCommentDto updateCommentDto);
    Task<List<GetCommentDto>> GetByCardComments(Guid CardId);
    Task RemoveAsync(string AppUserId, Guid CommentId);
}
