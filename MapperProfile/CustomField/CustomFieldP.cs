using AutoMapper;
using TaskMate.DTOs.CustomField;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.CustomField;

public class CustomFieldP : Profile
{
    public CustomFieldP()
    {
        CreateMap<CreateCustomFieldDto, CustomFields>().ReverseMap();
        CreateMap<CustomFields, UpdateCustomFieldDto>().ReverseMap();
        CreateMap<CustomFields, GetCustomFieldDto>()
            .ForMember(dest => dest.GetCustomFieldDropdownOptions, opt => opt.MapFrom(src => src.CustomFieldDropdownOptions))
            .ForMember(dest => dest.GetCustomFieldNumberDto, opt => opt.MapFrom(src => src.CustomFieldsNumbers))
            .ForMember(dest => dest.GetCustomFieldTextDto, opt => opt.MapFrom(src => src.CustomFieldsTexts))
            .ForMember(dest => dest.GetCustomFieldCheckboxDto, opt => opt.MapFrom(src => src.CustomFieldsCheckboxes))
            .ForMember(dest => dest.GetCustomFieldDateDto, opt => opt.MapFrom(src => src.CustomFieldsDates))
            .ReverseMap();
    }
}
