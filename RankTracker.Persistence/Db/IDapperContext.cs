using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankTracker.Persistence.Db;

public interface IDapperContext
{
    Task<IEnumerable<T>> QueryAsync<T>(string storedProc, object? parameters = null);
    Task<T> QuerySingleAsync<T>(string storedProc, object? parameters = null);
    Task<int> ExecuteAsync(string storedProc, object? parameters = null);
}
