using AutoMapper;
using TaskMate.DTOs.CardList;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.Cardlist;

public class CardListProfile:Profile
{
    public CardListProfile()
    {
        CreateMap<CardList, CreateCardListDto>().ReverseMap();
        CreateMap<CardList, UpdateeCardListDto>().ReverseMap();
        CreateMap<CardList, GetCardListDto>().ReverseMap();

    }
}
