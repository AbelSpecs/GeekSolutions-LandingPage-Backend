using GeekSolutions.Application.Interfaces.Persistence;
using GeekSolutions.Application.Interfaces.Services;
using GeekSolutions.Infrastructure.Services;
using GeekSolutions.Infrastructure.Persistence;
using GeekSolutions.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeekSolutions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IContactsRepository, ContactRepository>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}