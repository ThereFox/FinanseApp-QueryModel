using System.Data;
using System.Data.Common;
using Dapper;

namespace Persistense;

public class Class1
{
    private IDbConnection _connection;
    
    public void test()
    {
        _connection.Query<string>(
            "select * from user", 
            new { Age = "Test" },
            _connection.BeginTransaction());
    }
}