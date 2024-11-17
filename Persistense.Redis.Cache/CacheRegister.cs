using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Persistense.Interfaces;
using Persistense.Redis.Cashe.Stores;
using StackExchange.Redis;
using StackExchange.Redis.Configuration;

namespace Persistense.Redis.Cashe;

public static class CacheRegister
{
    public static IServiceCollection AddCache(this IServiceCollection services, string redisEndpointHost, int redisEndpointPort)
    {
        var endPoint= new DnsEndPoint(redisEndpointHost, redisEndpointPort);

        var configutation = new ConfigurationOptions()
        {
            AsyncTimeout = 300,
            EndPoints = [endPoint],
            HeartbeatInterval = TimeSpan.FromMinutes(15),
            ConnectRetry = 4,
            AbortOnConnectFail = false
        };

        services.AddScoped<IDatabase>(
            ex =>
            {
                var connection = ConnectionMultiplexer
                    .Connect(configutation)
                    .GetDatabase();
                return connection;
            }
            );
        
        services.AddScoped<IBillCacheStore, BillCacheStore>();
        services.AddScoped<IClientCacheStore, ClientCacheStore>();

        return services;
    }
}