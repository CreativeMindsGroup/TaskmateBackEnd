using AutoMapper;
using TaskMate.DTOs.Checkitem;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.Checkitemm;

public class CheckitemProfile:Profile
{
    public CheckitemProfile()
    {
        CreateMap<Checkitem, CreateCheckitemDto>().ReverseMap();
        CreateMap<Checkitem, UpdateCheckitemDto>().ReverseMap();
        CreateMap<Checkitem, GetCheckitemDto>().ReverseMap();
    }
}
