using TaskMate.Service.Abstraction;
using TaskMate.Service.Implementations;

namespace TaskMate.ExtensionsMethods.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        //user
        services.AddScoped<ITokenHandler, TokkenHandler>();
        services.AddScoped<IUserSerivce, AppUserService>();

        //Email
        services.AddScoped<IEmailService, EmailService>();

    }
}
