using AutoMapper;
using TaskMate.DTOs.CustomFieldDate;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomFieldNumberP;

public class CustomFieldNumberProfile:Profile
{
    public CustomFieldNumberProfile()
    {
        CreateMap<CustomFieldsDate, UpdateCustomFieldDateDto>();
        CreateMap<CustomFieldsDate, GetCustomFieldDateDto>();
    }
}
