using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace RankTracker.Persistence.Helpers;

public static class DapperHelper
{
    public static Task<int> ExecuteStoredProcAsync(
        this IDbConnection connection,
        string storedProc,
        object? parameters = null
    ) => connection.ExecuteAsync(
        storedProc,
        parameters,
        commandType: CommandType.StoredProcedure
    );

    public static Task<IEnumerable<T>> QueryStoredProcAsync<T>(
        this IDbConnection connection,
        string storedProc,
        object? parameters = null
    ) => connection.QueryAsync<T>(
        storedProc,
        parameters,
        commandType: CommandType.StoredProcedure
    );

    public static Task<T> QuerySingleStoredProcAsync<T>(
        this IDbConnection connection,
        string storedProc,
        object? parameters = null
    ) => connection.QuerySingleAsync<T>(
        storedProc,
        parameters,
        commandType: CommandType.StoredProcedure
    );
}
