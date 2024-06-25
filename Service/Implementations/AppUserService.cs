using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Text;
using TaskMate.Context;
using TaskMate.DTOs.Auth;
using TaskMate.DTOs.Users;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper;
using TaskMate.Helper.Auth;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations
{
    public class AppUserService : IUserSerivce
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppUserService(UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              AppDbContext context,
                              IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<SignUpResponse> Create(CreateUserDto registerDTO)
        {
            // Check if the current user has the required role
            var currentUserRole = await GetUserRole(registerDTO.AdminId);
            if (currentUserRole != Role.GlobalAdmin.ToString() && currentUserRole != Role.Admin.ToString())
            {
                throw new UnauthorizedAccessException("Only GlobalAdmin and Admin can create users.");
            }
            if (!Enum.TryParse(registerDTO.UserRole, out Role role) || !Enum.IsDefined(typeof(Role), role))
            {
                throw new ArgumentException("Invalid user role specified.");
            }
            AppUser appUser = new AppUser()
            {
                Email = registerDTO.Email,
                isActive = false, 
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerDTO.Password);
            if (!identityResult.Succeeded)
            {
                StringBuilder errorMessage = new();
                foreach (var error in identityResult.Errors)
                {
                    errorMessage.AppendLine(error.Description);
                }
                throw new RegistrationException(errorMessage.ToString());
            }

            // Assign the role to the created user
            var result = await _userManager.AddToRoleAsync(appUser, role.ToString());
            if (!result.Succeeded)
            {
                return new SignUpResponse
                {
                    StatusMessage = ExceptionResponseMessages.UserFailedMessage,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Return success response
            return new SignUpResponse
            {
                Errors = null,
                StatusMessage = ExceptionResponseMessages.UserSuccesMessage,
                UserEmail = appUser.Email
            };
        }
        public async Task<List<GetUserDto>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<GetUserDto>();

            foreach (var user in users)
            {
                var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                var userDto = _mapper.Map<GetUserDto>(user);
                userDto.Role = roleName;
                userDtos.Add(userDto);
            }
            return userDtos;
        }
        public async Task<DeleteUserDto> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new NotFoundException($"Failed to delete user with ID {userId}.");
            }
            return new DeleteUserDto(true, userId);
        }
        public async Task<GetUserDto> Update(EditUserDto EditDto)
        {
            var user = await _userManager.FindByIdAsync(EditDto.UserId.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with ID {EditDto.UserId} not found.");
            }
            if (!EditDto.Email.IsNullOrEmpty())
            {
                user.Email = EditDto.Email;
            }

            string RoleResult = await GetUserRole(EditDto.AdminId);

            if (RoleResult == Role.GlobalAdmin.ToString() || RoleResult == Role.Admin.ToString())
            {
                if (EditDto.UserRole != null && Enum.TryParse(EditDto.UserRole, out Role newRole))
                {
                    if (!await _roleManager.RoleExistsAsync(newRole.ToString()))
                    {
                        throw new NotFoundException("Role not found");
                    }

                    var currentUserRoles = await _userManager.GetRolesAsync(user);

                    await _userManager.RemoveFromRolesAsync(user, currentUserRoles);
                    await _userManager.AddToRoleAsync(user, newRole.ToString());
                }
                else
                {
                    throw new NotFoundException("Role not found");
                }
            }

            if (RoleResult == Role.GlobalAdmin.ToString() && !EditDto.Password.IsNullOrEmpty())
            {
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, EditDto.Password);
                user.PasswordHash = newPasswordHash;
            }
            else if (RoleResult != Role.GlobalAdmin.ToString() && !EditDto.Password.IsNullOrEmpty())
            {
                throw new UnauthorizedAccessException("Only GlobalAdmin can change passwords.");
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user.");
            }

            var updatedUserDto = _mapper.Map<GetUserDto>(user);
            return updatedUserDto;
        }


        public async Task<string> GetUserRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<GetUserDto> GetById(Guid id)
        {
            string stringId = id.ToString();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == stringId);

            if (user == null)
            {
                throw new NotFoundException("user not found!");
            }

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var userDto = _mapper.Map<GetUserDto>(user);
            userDto.Role = roleName;
            return userDto;
        }

        public async Task<List<GetUserDto>> SearchUserByEmailorUsername(string value)
        {
            var users = await _context.Users
                .Where(u => u.Email.Contains(value) || u.UserName.Contains(value))
                .Take(7)
                .ToListAsync();

            var userDtos = _mapper.Map<List<GetUserDto>>(users);
            return userDtos;
        }

        public async Task<bool> CheckIsAdmin(string AdminId)
        {
            var admin = await _context.AppUsers.FirstOrDefaultAsync(x=>x.Id==AdminId);
            if (admin is null) throw new NotFoundException("Admin Not Found");

            var roleName = (await _userManager.GetRolesAsync(admin)).FirstOrDefault();
            if (roleName.ToString() == Role.GlobalAdmin.ToString() || roleName.ToString() == Role.Admin.ToString())  return true;
            return false;
        }
    }
}
