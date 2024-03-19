using AutoMapper;
using TaskMate.DTOs.Card;

namespace TaskMate.MapperProfile.Card;

public class CardProfile:Profile
{
    public CardProfile()
    {
        CreateMap<CreateCardDto, TaskMate.Entities.Card>().ReverseMap();
        CreateMap<UpdateCardDto, TaskMate.Entities.Card>().ReverseMap();
        CreateMap<GetCardDto, TaskMate.Entities.Card>().ReverseMap();
    }
}
