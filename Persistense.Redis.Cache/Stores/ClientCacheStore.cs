using Application.DTOs;
using Common;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Persistense.Interfaces;
using StackExchange.Redis;

namespace Persistense.Redis.Cashe.Stores;

public class ClientCacheStore : IClientCacheStore
{
    private readonly IDatabase _database;

    public ClientCacheStore(IDatabase database)
    {
        _database = database;
    }

    public async Task<Result<ClientShortInfoDTO>> GetByIdAsync(Guid clientId)
    {
        try
        {
            var getClientResult = await _database.StringGetAsync(clientId.ToString());

            if (getClientResult.HasValue == false)
            {
                return Result.Failure<ClientShortInfoDTO>($"Client with id {clientId} not found");
            }

            var deserializeResult = ResultJsonDeserialiser.Deserialise<ClientShortInfoDTO>((string)getClientResult.Box());
            
            return deserializeResult;
        }
        catch (Exception ex)
        {
            return Result.Failure<ClientShortInfoDTO>(ex.Message);
        }
    }

    public async Task<Result> SaveById(Guid clientId, ClientShortInfoDTO client)
    {
        try
        {
            var serialiseClient = JsonConvert.SerializeObject(client);

            var saveResult = await _database.StringSetAsync(
                clientId.ToString(),
                serialiseClient,
                TimeSpan.FromMinutes(5),
                true
            );

            return Result.SuccessIf(saveResult, $"Error whileAdding");
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}