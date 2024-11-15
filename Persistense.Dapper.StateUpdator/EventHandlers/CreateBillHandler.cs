using System.Data;
using Application.Events.Realisation;
using Application.Handlers.Abstcations;
using CSharpFunctionalExtensions;
using Dapper;

namespace Persistense.Dapper.StateUpdator.EventHandlers;

public class CreateBillHandler : IEventHadler<BillCreatedEvent>
{
    private readonly IDbConnection _connection;

    public CreateBillHandler(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Result> HandleAsync(BillCreatedEvent @event)
    {
        var checkBillNotCreating = await checkUnContainingBill(@event.BillId);

        if (checkBillNotCreating.IsFailure)
        {
            return checkBillNotCreating;
        }
        
        var checkClientResult = await checkContainingClient(@event.OwnerId);

        if (checkClientResult.IsFailure)
        {
            return checkClientResult;
        }
        
        return await createBill(@event);
    }

    private async Task<Result> createBill(BillCreatedEvent @event)
    {
        var createBillSQLRequest = $@"
        INSERT INTO bills
        VALUES
        (@BillId, @OwnerId, 0)
        ";

        var changedRows = await _connection.ExecuteAsync(createBillSQLRequest, new { @BillId = @event.BillId, @OwnerId = @event.OwnerId });

        if (changedRows != 1)
        {
            return Result.Failure("Error");
        }
        
        return Result.Success();
    }

    private async Task<Result> checkContainingClient(Guid Id)
    {
        var HaveClientSQLRequest = $@"
            SELECT COUNT(*) 
            FROM clients
            WHERE Id = @clientId
            ";

        var checkClientHaveResult = await _connection
            .ExecuteScalarAsync<int>(
                HaveClientSQLRequest, new { @clientId = Id }
                );

        if (checkClientHaveResult == 0)
        {
            return Result.Failure($"Client with Id {Id} dont exists");
        }

        return Result.Success();
    }

    private async Task<Result> checkUnContainingBill(Guid Id)
    {
        var alreadyHaveSQLRequest = $@"
            SELECT COUNT(*) 
            FROM bills
            WHERE Id = @billId
            ";

        var checkContainingResult = await _connection
            .ExecuteScalarAsync<int>(
                alreadyHaveSQLRequest, new { @billId = Id }
                );

        if (checkContainingResult != 0)
        {
            return Result.Failure($"Bill with Id {Id} already exists");
        }

        return Result.Success();
    }
}