using Application.Events.Abstractions;
using Application.Handlers.Abstcations;
using Confluent.Kafka;
using Infrastructure.StateUpdator.Transaction;
using Infrastructure.StateUpdator.ValueConverter;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.StateUpdator;

public static class StateUpdatorRegister
{
    public static IServiceCollection AddStateUpdating(this IServiceCollection services)
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "TestGroup",
            EnableAutoCommit = false,
            AllowAutoCreateTopics = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
            
        var consumer = new ConsumerBuilder<Null, string>(config).Build();

        services.AddSingleton<IConsumer<Null, string>>(consumer);
        
        services.AddScoped<KafkaConsumer<Null, string>>();
        services.AddScoped<IKafkaTransaction<string>, KafkaHandlTransaction<Null, string>>();
        services.AddScoped<IValueConverter<string>, JsonValueConverter>();
        
        return services;
    }

    public static IServiceCollection RegistrateStateUpdateEventHandler<TEvent, THandler>(this IServiceCollection services,
        string topicName)
        where TEvent : IDBStateChangeEvent
        where THandler : class, IEventHadler<TEvent>
    {

        services.AddScoped<IEventHadler<TEvent>, THandler>();
        services.AddScoped<StateUpdator<TEvent>>(
            ex =>
            {
                var handler = ex.GetRequiredService<IEventHadler<TEvent>>();
                var kafkaConsumer = ex.GetRequiredService<KafkaConsumer<Null, string>>();
                var parser = ex.GetRequiredService<IValueConverter<string>>();
                
                return new StateUpdator<TEvent>(
                    topicName,
                    kafkaConsumer.CreateAtomarityTransaction(),
                    handler,
                    parser
                );
            }
        );

        return services;
    }
}