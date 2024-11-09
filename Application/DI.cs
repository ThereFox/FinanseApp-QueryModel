using Application.Handlers.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BillRequestHandler>();
        services.AddScoped<ClientRequestHandler>();
        
        return services;
    }
}