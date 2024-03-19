using AutoMapper;
using TaskMate.DTOs.Workspace;

namespace TaskMate.MapperProfile.Workspace;

public class WorkspaceProfile:Profile
{
    public WorkspaceProfile()
    {
        CreateMap<TaskMate.Entities.Workspace, CreateWorkspaceDto>().ReverseMap();
        CreateMap<TaskMate.Entities.Workspace, UpdateWorkspaceDto>().ReverseMap();
        CreateMap<TaskMate.Entities.Workspace, GetWorkspaceDto>().ReverseMap();
        CreateMap<TaskMate.Entities.Workspace, GetWorkspaceInBoardDto>()
            .ForMember(dest => dest.getBoardsDtos, opt => opt.MapFrom(src => src.Boards)).ReverseMap();
    }
}
