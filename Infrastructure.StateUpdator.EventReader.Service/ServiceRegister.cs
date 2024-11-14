using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.StateUpdator.EventReader.Service;

public static class ServiceRegister
{
    public static IServiceCollection AddBackgroundDBUpdator(this IServiceCollection services)
    {
        services.AddHostedService<EventReaderService>();

        return services;
    }
}