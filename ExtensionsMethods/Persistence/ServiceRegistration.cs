using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
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
        services.AddIdentity<AppUser, IdentityRole>(Options =>
        {
            Options.User.RequireUniqueEmail = true;
            Options.Password.RequiredLength = 8;
            Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
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
        services.AddScoped<ILabelService, LabelService>();
        services.AddScoped<IChecklistService, ChecklistService>();
        services.AddScoped<ICheckitemService, CheckitemService>();


        services.AddScoped<ICustomFieldsService, CustomFieldsService>();

    }

}
