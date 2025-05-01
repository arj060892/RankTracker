using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankTracker.Service.Clients;

public interface ISearchEngineClient
{
    Task<List<(string Domain, int Position)>> GetResultsWithPositionsAsync(string query);
}
