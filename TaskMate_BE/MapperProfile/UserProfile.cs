using AutoMapper;
using TaskMate.DTOs.Slider;
using TaskMate.DTOs.Users;
using TaskMate.Entities;

namespace TaskMate.MapperProfile
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, GetUserDto>().ReverseMap();
        }
    }
}
