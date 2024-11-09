using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Persistense;

public static class DI
{
    public static IServiceCollection AddPersistense(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(ex =>
        {
            var connection = new SqlConnection(connectionString);
            return connection;
        });

        return services;
    }
}