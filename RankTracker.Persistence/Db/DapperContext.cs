using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace RankTracker.Persistence.Db
{
    public class DapperContext : IDapperContext
    {
        private readonly IDbConnectionFactory _factory;

        public DapperContext(IDbConnectionFactory factory)
            => _factory = factory;

        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProc, object? parameters = null)
        {
            using var conn = _factory.CreateConnection();
            return await conn.QueryAsync<T>(
                storedProc,
                parameters,
                commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);
        }

        public async Task<T> QuerySingleAsync<T>(string storedProc, object? parameters = null)
        {
            using var conn = _factory.CreateConnection();
            return await conn.QuerySingleAsync<T>(
                storedProc,
                parameters,
                commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);
        }

        public async Task<int> ExecuteAsync(string storedProc, object? parameters = null)
        {
            using var conn = _factory.CreateConnection();
            return await conn.ExecuteAsync(
                storedProc,
                parameters,
                commandType: CommandType.StoredProcedure
            ).ConfigureAwait(false);
        }
    }
}
