using AutoMapper;
using TaskMate.DTOs.CustomField;
using TaskMate.DTOs.CustomFieldCheckbox;
using TaskMate.DTOs.CustomFieldNumber;
using TaskMate.DTOs.DropDownOptionsDTO;
using TaskMate.Entities;

public class CustomFieldP : Profile
{
    public CustomFieldP()
    {
        CreateMap<CustomFields, GetCustomFieldDto>()
            .ForMember(dest => dest.CheckboxDto, opt => opt.MapFrom(src => src.Checkbox)) // Map Checkbox to CheckboxDto
            .ForMember(dest => dest.NumberDto, opt => opt.MapFrom(src => src.Number)) // Map Number to NumberDto
            .ForMember(dest => dest.DropDownDto, opt => opt.MapFrom(src => src.DropDown))
            ;

        CreateMap<CustomFieldsCheckbox, GetCustomFieldCheckboxDto>().ReverseMap();
        CreateMap<CustomFieldsNumber, GetCustomFiledNumber>().ReverseMap();

        CreateMap<DropDown, GetDropDownDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.SelectedId, opt => opt.MapFrom(src => src.SelectedId))
            .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.OptionName))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.DropDownOptions, opt => opt.MapFrom(src => src.DropDownOptions)).ReverseMap(); // Map DropDownOptions to DropDownOptionsDto

        CreateMap<DropDownOptions, GetDropDownOptionsDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.OptionName))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ReverseMap();

        // Add this mapping configuration
        CreateMap<DropDownOptionsCreateDTO, DropDownOptions>()
            .ForMember(dest => dest.OptionName, opt => opt.MapFrom(src => src.OptionName))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ReverseMap();
    }
}
