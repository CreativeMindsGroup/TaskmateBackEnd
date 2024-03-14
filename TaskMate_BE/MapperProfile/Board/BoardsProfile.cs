using AutoMapper;
using TaskMate.DTOs.Boards;
using TaskMate.Entities;

namespace TaskMate.MapperProfile.Board;

public class BoardsProfile:Profile
{
    public BoardsProfile()
    {
        CreateMap<Boards, CreateBoardsDto>().ReverseMap();
        CreateMap<Boards, UpdateBoardsDto>().ReverseMap();
        CreateMap<Boards, GetBoardsDto>()
            .ForMember(dest => dest.getCardListDtos, opt => opt.MapFrom(src=>src.CardLists)).ReverseMap();
    }
}
