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
    
    public async Task<IEnumerable<ClientShortInfoDTO>> GetAll()
    {
        var sqlRequest = @"SELECT * FROM clients";
        
        return await _connection.QueryAsync<ClientShortInfoDTO>(sqlRequest);
    }
}