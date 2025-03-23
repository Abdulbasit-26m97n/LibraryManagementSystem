using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {

            // Register DbContext
            services.AddDbContext<LMSDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Register Repositories
            services.AddScoped<IBookRepository, BookRepository>()
            .AddScoped<IUserRepository, UserRepository>();

            // Register Services
            services.AddScoped<IJwtService, JwtService>(); 

            return services;
        }
    }
}
