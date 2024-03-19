using AutoMapper;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomFieldCheckboxP;

public class CustomFieldCheckboxProfile:Profile
{
    public CustomFieldCheckboxProfile()
    {
        CreateMap<CustomFieldsCheckbox, UpdateCustomFieldCheckboxDto>().ReverseMap();
        CreateMap<CustomFieldsCheckbox, GetCustomFieldCheckboxDto>().ReverseMap();
    }
}
