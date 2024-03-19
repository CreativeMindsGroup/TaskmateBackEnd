using AutoMapper;
using TaskMate.DTOs.Label;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.Label;

public class LabelProfile:Profile
{
    public LabelProfile()
    {
        CreateMap<Labels, CreateLabelDto>().ReverseMap();
        CreateMap<Labels, GetLabelDto>().ReverseMap();
    }
}
