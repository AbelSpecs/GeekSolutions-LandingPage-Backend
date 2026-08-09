using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using MediatR;

namespace GeekSolutions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        //services.AddScoped<IContactService, ContactService>();

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}