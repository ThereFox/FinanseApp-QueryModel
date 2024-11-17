using Application.DTOs;
using Common;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Persistense.Interfaces;
using StackExchange.Redis;

namespace Persistense.Redis.Cashe.Stores;

public class BillCacheStore : IBillCacheStore
{
    private readonly IDatabase _database;

    public BillCacheStore(IDatabase database)
    {
        _database = database;
    }

    public async Task<Result<List<BillShortInfoDTO>>> GetAllByOwner(Guid OwnerId)
    {
        try
        {
            var databyKey = await _database.StringGetAsync($"Owner{OwnerId.ToString()}");

            if (databyKey.HasValue == false)
            {
                return Result.Failure<List<BillShortInfoDTO>>($"dont contain value for owner by Id {OwnerId}");
            }
            
            var tryDeserializeBill = ResultJsonDeserialiser.Deserialise<List<BillShortInfoDTO>>((string)databyKey.Box());

            return tryDeserializeBill;
        }
        catch (Exception e)
        {
            return Result.Failure<List<BillShortInfoDTO>>(e.Message);
        }
    }

    public async Task<Result<BillShortInfoDTO>> GetById(Guid id)
    {
        try
        {
            var databyKey = await _database.StringGetAsync(id.ToString());

            if (databyKey.HasValue)
            {
                return Result.Failure<BillShortInfoDTO>($"dont contain value by Id {id}");
            }
            
            var tryDeserializeBill = ResultJsonDeserialiser.Deserialise<BillShortInfoDTO>((string)databyKey.Box());

            return tryDeserializeBill;
        }
        catch (Exception e)
        {
            return Result.Failure<BillShortInfoDTO>(e.Message);
        }
    }

    public async Task<Result> SaveById(Guid id, BillShortInfoDTO bill)
    {
        try
        {
            var serialiseBill = JsonConvert.SerializeObject(bill);

            var saveResult = await _database.StringSetAsync(
                id.ToString(),
                serialiseBill,
                TimeSpan.FromMinutes(3),
                true
                );

            return Result.SuccessIf(saveResult, $"Error whileAdding");
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public async Task<Result> SaveByOwnerId(Guid OwnerId, List<BillShortInfoDTO> bills)
    {
        try
        {
            var serialiseBill = JsonConvert.SerializeObject(bills);

            var saveResult = await _database.StringSetAsync(
                $"Owner{OwnerId.ToString()}",
                serialiseBill,
                TimeSpan.FromMinutes(3),
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