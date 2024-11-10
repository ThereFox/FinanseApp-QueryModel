using Microsoft.Extensions.DependencyInjection;

namespace Application.StateUpdator.DIs;

public static class EventHandlerRegister
{
    public static IServiceCollection AddStateUpdator<TInput, THandler>(this IServiceCollection services, string topicName)
    {
        services.AddScoped()
    }
}