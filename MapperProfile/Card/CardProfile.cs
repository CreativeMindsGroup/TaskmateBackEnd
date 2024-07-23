using AutoMapper;
using TaskMate.DTOs.Card;
using TaskMate.Entities;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<CreateCardDto, TaskMate.Entities.Card>().ReverseMap();
        CreateMap<UpdateCardDto, TaskMate.Entities.Card>().ReverseMap();
        CreateMap<Card, GetArchivedCardDto>().ReverseMap();
        CreateMap<GetCardDto, TaskMate.Entities.Card>().ReverseMap()
             .ForMember(dest => dest.AppUsers, opt => opt.MapFrom(src => src.AppUsersCards.Select(ac => ac.AppUser)))
             .ForMember(dest => dest.GetCustomFieldDto, opt => opt.MapFrom(src => src.CustomFields)); 
        CreateMap<CardAttachment, CardAttachmentDto>().ReverseMap();  // This should theoretically work
    }
}
