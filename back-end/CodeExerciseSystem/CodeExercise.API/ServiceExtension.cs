using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.Services;
using CodeExercise.Infrastructure.Persistence.Repositories;

namespace CodeExercise.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IProblemRepository, ProblemRepository>();
            services.AddScoped<IProgrammingLanguageRepository, ProgrammingLanguageRepository>();
            services.AddScoped<ITestCaseRepository, TestCaseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            // Register services
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IProblemService, ProblemService>();
            services.AddScoped<IUserService, UserService>();

            //var storagePath = configuration["StoragePath"];
            //if (string.IsNullOrEmpty(storagePath))
            //{
            //    throw new InvalidOperationException("StoragePath is not configured in environment variables.");
            //}
            //services.AddScoped<IStudyMaterialService>(provider =>
            //    new StudyMaterialService(
            //        provider.GetRequiredService<IStudyMaterialRepository>(),
            //        storagePath
            //    ));

            return services;
        }
    }
}
