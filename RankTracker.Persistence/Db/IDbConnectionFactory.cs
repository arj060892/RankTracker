using System.Data;

namespace RankTracker.Persistence.Db;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
