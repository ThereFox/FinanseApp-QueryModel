using System.Data;
using Application.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Persistense.Stores;

namespace Persistense;

public static class DI
{
    public static IServiceCollection AddPersistense(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(ex =>
        {
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        });

        services.AddScoped<IClientStore, ClientStore>();
        services.AddScoped<IBillStore, BillStore>();
        
        return services;
    }
}