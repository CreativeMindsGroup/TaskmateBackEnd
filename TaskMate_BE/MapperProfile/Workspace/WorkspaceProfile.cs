using AutoMapper;
using TaskMate.DTOs.Workspace;

namespace TaskMate.MapperProfile.Workspace;

public class WorkspaceProfile:Profile
{
    public WorkspaceProfile()
    {
        CreateMap<CreateWorkspaceDto, TaskMate.Entities.Workspace>().ReverseMap();
        CreateMap<UpdateWorkspaceDto, TaskMate.Entities.Workspace>().ReverseMap();
        CreateMap<GetWorkspaceDto, TaskMate.Entities.Workspace>().ReverseMap();
    }
}
