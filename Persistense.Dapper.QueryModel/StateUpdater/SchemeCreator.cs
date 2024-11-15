using System.Data;
using Dapper;

namespace Persistense.StateUpdater;

public class SchemeCreator
{
    private readonly IDbConnection _connection;

    public SchemeCreator(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task CreateScheme()
    {
        var createClientsTableSQLCommand = $@"
            CREATE TABLE IF NOT EXISTS clients
            ( 
                id UUID PRIMARY KEY,
                name VARCHAR(255) NOT NULL
            )
        ";
        var createBillsTableSQLCommand = $@"

            CREATE TABLE IF NOT EXISTS bills
            (
                id UUID PRIMARY KEY,
                ownerId UUID REFERENCES clients (id),
                amount INTEGER NOT NULL
            )
        ";

        var createClientsTableResult = await _connection.ExecuteAsync(createClientsTableSQLCommand);
        var createBillsTableResult = await _connection.ExecuteAsync(createBillsTableSQLCommand);
        
        return;
        
    }
    
}