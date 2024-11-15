using System.Data;
using Application.DTOs;
using Application.Stores;
using CSharpFunctionalExtensions;
using Dapper;

namespace Persistense.Stores;

public class BillStore : IBillStore
{
    private readonly IDbConnection _connection;

    public BillStore(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<IEnumerable<BillPublicShortInfoDTO>> GetAll()
    {
        var sqlCommandBase = 
            $@"
            SELECT bills.{"\"Id\""}, 0, clients.{"\"Name\""} as OwnerName
            FROM bills
            INNER JOIN clients
                ON bills.{"\"OwnerId\""} = clients.{"\"Id\""}
            ";
        
        var result = await _connection.QueryAsync<BillPublicShortInfoDTO>(sqlCommandBase);
        
        return result;
    }

    public Task<IEnumerable<BillPublicShortInfoDTO>> GetWithBalanse()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<BillPublicShortInfoDTO>> GetById(Guid id)
    {
        return Result.Failure<BillPublicShortInfoDTO>("Error");
    }
}