using AutoMapper;
using TaskMate.DTOs.CustomFieldDropdownOption;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomFieldDropdownOptionp;

public class CustomFieldDropdownOptionProfile:Profile
{
    public CustomFieldDropdownOptionProfile()
    {
        CreateMap<CustomFieldDropdownOptions, CreateCustomFieldDropdownOption>().ReverseMap();
        CreateMap<CustomFieldDropdownOptions, UpdateCustomFieldDropdownOption>().ReverseMap();
        CreateMap<CustomFieldDropdownOptions, GetCustomFieldDropdownOption>().ReverseMap();
    }
}
