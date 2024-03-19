using AutoMapper;
using TaskMate.DTOs.Comment;
using TaskMate.Entities;

namespace TaskMate.MapperProfile;

public class CommentProfile:Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CreateCommentDto>().ReverseMap();
        CreateMap<Comment, UpdateCommentDto>().ReverseMap();
        CreateMap<Comment, GetCommentDto>().ReverseMap();
    }
}
