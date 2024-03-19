using AutoMapper;
using TaskMate.DTOs.Checklist;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.Checklistt;

public class ChecklistProfile:Profile
{
    public ChecklistProfile()
    {
        CreateMap<Checklist, CreateChecklistDto>().ReverseMap();
        CreateMap<Checklist, UpdateChecklistDto>().ReverseMap();
        CreateMap<Checklist, GetChecklistDto>()
           .ForMember(dest => dest.getCheckitemDtos, opt => opt.MapFrom(src => src.Checkitems)).ReverseMap();

    }
}
