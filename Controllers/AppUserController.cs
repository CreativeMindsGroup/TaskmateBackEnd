using Microsoft.AspNetCore.Mvc;
using TaskMate.DTOs.Users;
using TaskMate.Helper;
using TaskMate.Service.Abstraction;

namespace TaskMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IUserSerivce _userSerivce;

        public AppUserController(IUserSerivce userSerivce)
        {
            _userSerivce = userSerivce;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto UserDto)
        {
            ArgumentNullException.ThrowIfNull(UserDto, ExceptionResponseMessages.ParametrNotFoundMessage);
            var Response = await _userSerivce.Create(UserDto);
            if (Response.Errors != null)
            {
                if (Response.Errors.Count > 0)
                {
                    return BadRequest(Response.Errors);
                }
            }
            return Ok(Response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var getUserDto = await _userSerivce.GetAllUsers(); 
            return Ok(getUserDto);
        }


        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteUser(Guid UserId)
        {
            var Result = await _userSerivce.Delete(UserId);
            return Ok(Result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(EditUserDto EditDto)
        {
            var Result = await _userSerivce.Update(EditDto);
            return Ok(Result);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var getUserDto = await _userSerivce.GetById(id);
            return Ok(getUserDto);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckAdmin(string AdminId)
        {
            var isAdmin = await _userSerivce.CheckIsAdmin(AdminId);
            return Ok(isAdmin);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchUserByEmailorUsername(string value)
        {
            var getUserDto = await _userSerivce.SearchUserByEmailorUsername(value);
            return Ok(getUserDto);
        }
    }
}
