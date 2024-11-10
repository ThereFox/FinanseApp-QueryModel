using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.StateUpdator;

public static class StateUpdatorRegister
{
    public static IServiceCollection AddStateUpdator(this IServiceCollection services)
    {
        services.AddSingleton<KafkaConsumer<Null, string>>();

        return services;
    }
}