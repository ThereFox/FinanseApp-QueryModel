using System.Data;
using Application.Events.Realisation;
using Application.Handlers.Abstcations;
using CSharpFunctionalExtensions;
using Dapper;

namespace Persistense.Dapper.StateUpdator.EventHandlers;

public class CreateClientHandler : IEventHadler<ClientCreatedEvent>
{
    private readonly IDbConnection _connection;

    public CreateClientHandler(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Result> HandleAsync(ClientCreatedEvent @event)
    {
        try
        {
            var transaction = _connection.BeginTransaction();
            
            var hasntClient = await clientUnhaving(@event.ClientId);

            if (hasntClient.IsFailure)
            {
                transaction.Rollback();
                return hasntClient;
            }

            var createClientResult = await createClient(@event);

            if (createClientResult.IsFailure)
            {
                transaction.Rollback();
                return createClientResult;
            }

            transaction.Commit();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private async Task<Result> clientUnhaving(Guid clientId)
    {
        var containingClientSQLRequest = @$"
            SELECT COUNT(*)
            FROM clients
            WHERE Id = @id
        ";
        
        var checkContainingResult = await _connection.ExecuteScalarAsync<int>(containingClientSQLRequest, new { id = clientId });
        
        return Result.SuccessIf(checkContainingResult == 0, "Already exists");
    }

    private async Task<Result> createClient(ClientCreatedEvent @event)
    {
        var createClientSQLCommand = @$"
            INSERT INTO clients
            (Id, Name)
            VALUES 
            (@ClientId, @ClientName)
        ";
        var executeResult = await _connection.ExecuteAsync(createClientSQLCommand, new { @event.ClientId, @event.ClientName });
        
        return Result.FailureIf(executeResult == 0, "Already exists");
    }
}