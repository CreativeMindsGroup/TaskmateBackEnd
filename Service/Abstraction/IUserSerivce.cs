using TaskMate.DTOs.Auth;
using TaskMate.DTOs.Users;
using TaskMate.Entities;
using TaskMate.Helper.Auth;

namespace TaskMate.Service.Abstraction
{
    public interface IUserSerivce
    {
        Task<SignUpResponse> Create(CreateUserDto registerDTO);
        Task<List<GetUserDto>> GetAllUsers();
        Task<DeleteUserDto> Delete(Guid UserId);
        Task<GetUserDto> Update(EditUserDto EditDto);
        Task<GetUserDto> GetById(Guid id);
        Task<List<GetUserDto>> SearchUserByEmailorUsername(string value);
        Task<bool> CheckIsAdmin(string AdminId);
    }
}
