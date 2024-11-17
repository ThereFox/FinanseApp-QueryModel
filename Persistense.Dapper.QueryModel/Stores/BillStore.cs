using System.Data;
using Application.DTOs;
using Application.Stores;
using CSharpFunctionalExtensions;
using Dapper;
using Persistense.Interfaces;

namespace Persistense.Stores;

public class BillStore : IBillStore
{
    private readonly IDbConnection _connection;
    private readonly IBillCacheStore _cacheStore;
    
    public BillStore(IDbConnection connection, IBillCacheStore cacheStore)
    {
        _connection = connection;
        _cacheStore = cacheStore;
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

    public async Task<Result<List<BillShortInfoDTO>>> GetAllByOwner(Guid OwnerId)
    {
        var getByOwnerResult = await _cacheStore.GetAllByOwner(OwnerId);

        if (getByOwnerResult.IsSuccess)
        {
            return getByOwnerResult.Value;
        }
        
        var sqlCommandBase = @"
            SELECT clients.Id, bills.amount, clients.Name
            FROM bills
                INNER JOIN clients
                ON clients.Id = bills.OwnerId
            Where OwnerId = @OwnerId
            ";
        var result = await _connection.QueryAsync<BillShortInfoDTO>(sqlCommandBase, OwnerId);
        return result.ToList();
    }

    public async Task<IEnumerable<BillShortInfoDTO>> GetWithBalanse()
    {
        var sqlCommandBase = @"
            SELECT clients.Id, bills.amount, clients.Name
            FROM bills
                INNER JOIN clients
                ON clients.Id = bills.OwnerId
            Where bills.Amount > 0
            ";
        var result = await _connection.QueryAsync<BillShortInfoDTO>(sqlCommandBase);
        return result;
    }

    public async Task<Result<BillShortInfoDTO>> GetById(Guid id)
    {
        var sqlCommandBase = @"
            SELECT clients.Id, bills.amount, clients.Name
            FROM bills
                INNER JOIN clients
                ON clients.Id = bills.OwnerId
            Where bills.Id = @billId
            ";
        var result = await _connection.QuerySingleAsync<BillShortInfoDTO>(sqlCommandBase, id);
        return result;
    }
}