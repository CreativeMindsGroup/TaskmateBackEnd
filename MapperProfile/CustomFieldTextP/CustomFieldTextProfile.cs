using AutoMapper;
using TaskMate.DTOs.CustomFieldText;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomFieldTextP;

public class CustomFieldTextProfile:Profile
{
    public CustomFieldTextProfile()
    {
        CreateMap<CustomFieldsText, UpdateCustomFieldTextDto>().ReverseMap();
        CreateMap<CustomFieldsText, GetCustomFieldTextDto>().ReverseMap();
    }
}
