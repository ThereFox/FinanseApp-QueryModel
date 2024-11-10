using System.Data;
using Application.Events.Realisation;
using Application.Handlers.Abstcations;
using CSharpFunctionalExtensions;
using Dapper;

namespace Persistense.Dapper.StateUpdator.EventHandlers;

public class BillAmountChangeHandler : IEventHadler<BillAmountChangeEvent>
{
    private readonly IDbConnection _connection;

    public BillAmountChangeHandler(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Result> HandleAsync(BillAmountChangeEvent @event)
    {
        try
        {
            var checkContainingResult = await haveBillWithId(@event.BillId);

            if (checkContainingResult.IsFailure)
            {
                return checkContainingResult;
            }

            var updateResult = await UpdateAmount(@event);

            return updateResult;
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
    
    private async Task<Result> haveBillWithId(Guid Id)
    {
        var containWithThisBillId = $@"
            SELECT COUNT(*) 
            FROM {"\"Bills\""}
            WHERE Id = @billId
            ";

        var checkContainingResult = await _connection.ExecuteScalarAsync<int>(
            containWithThisBillId,
            new { @billId = Id }
        );

        if (checkContainingResult == 0)
        {
            return Result.Failure($"Bill with Id {Id} dont exists");
        }

        return Result.Success();
    }

    private async Task<Result> UpdateAmount(BillAmountChangeEvent @event)
    {
        var updateAmountSQLCommand = $@"

            UPDATE {"\"Bills\""}
            SET Amount = @amount
            WHERE Id = @BillId
        ";

        var countOfChangeRows = await _connection.ExecuteAsync(updateAmountSQLCommand,
            new { @amount = @event.newAmount, @BillId = @event.BillId });

        if (countOfChangeRows != 1)
        {
            return Result.Failure("error");
        }

        return Result.Success();
    }
}