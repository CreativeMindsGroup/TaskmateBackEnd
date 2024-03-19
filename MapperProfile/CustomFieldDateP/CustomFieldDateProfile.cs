using AutoMapper;
using TaskMate.DTOs.CustomFieldDate;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomFieldDateP;

public class CustomFieldDateProfile:Profile
{
    public CustomFieldDateProfile()
    {
        CreateMap<CustomFieldsDate, UpdateCustomFieldDateDto>().ReverseMap();
        CreateMap<CustomFieldsDate, GetCustomFieldDateDto>().ReverseMap();
    }
}
