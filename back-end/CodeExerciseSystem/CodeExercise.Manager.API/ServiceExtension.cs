using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.Services;
using CodeExercise.Infrastructure.Persistence.Repositories;

namespace CodeExercise.Manager.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddScoped<IUserRepository, UserRepository>();
            


            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
           
            return services;
        }
    }
}
