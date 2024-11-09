using System.Data;
using Application.DTOs;
using Application.Stores;
using Dapper;

namespace Persistense.Stores;

public class ClientStore : IClientStore
{
    private readonly IDbConnection _connection;

    public ClientStore(IDbConnection connection)
    {
        _connection = connection;
    }

    private readonly string getAllSQLRequest = "SELECT * FROM \"Clients\"";
    
    public async Task<IEnumerable<ClientShortInfoDTO>> GetAll()
    {
        return await _connection.QueryAsync<ClientShortInfoDTO>(getAllSQLRequest);
    }
}