using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Slider;
using TaskMate.Entities;
using TaskMate.MapperProfile;
using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.ExtensionsMethods.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(services.BuildServiceProvider().GetService<IConfiguration>().GetConnectionString("Default"));
        });
        services.AddIdentity<AppUser, IdentityRole>(Options =>
        {
            Options.User.RequireUniqueEmail = true;
            Options.Password.RequireNonAlphanumeric = true;
            Options.Password.RequiredLength = 8;
            Options.Password.RequireDigit = true;
            Options.Password.RequireUppercase = true;
            Options.Password.RequireLowercase = true;
            Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            Options.Lockout.MaxFailedAccessAttempts = 3;
            Options.Lockout.AllowedForNewUsers = true;
        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
        //Mapper
        services.AddAutoMapper(typeof(SliderProfile).Assembly);

        //Validator
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<SliderCreateDTO>();


        //Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserSerivce, AppUserService>();
        services.AddScoped<IWorkspaceService, WorkspaceService>();
        services.AddScoped<IBoardsService, BoardsService>();
        services.AddScoped<ICardListService, CardListService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ICommentService, CommentService>();


    }



}
