using AutoMapper;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.Entities;

public class CustomFieldP : Profile
{
    public CustomFieldP()  
    {
        CreateMap<CustomFields, GetCustomFieldDto>()
            .ReverseMap();
        CreateMap<CustomFieldsCheckbox, GetCustomFieldCheckboxDto>().ReverseMap();
        CreateMap<CustomFieldsNumber, GetCustomFiledNumber>().ReverseMap();
    }
}
