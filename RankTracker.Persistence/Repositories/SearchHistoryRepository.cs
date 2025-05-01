using System.Collections.Generic;
using System.Threading.Tasks;
using RankTracker.Persistence.Db;
using RankTracker.Persistence.Models;

namespace RankTracker.Persistence.Repositories;

public class SearchHistoryRepository : ISearchHistoryRepository
{
    private readonly IDapperContext _context;

    public SearchHistoryRepository(IDapperContext context)
        => _context = context;

    public Task<int> InsertSearchHistoryAsync(
        string engineName,
        string queryText,
        string targetUrl,
        string positions
    ) => _context.ExecuteAsync(
        storedProc: "ranking.InsertSearchHistory",
        parameters: new
        {
            EngineName = engineName,
            QueryText = queryText,
            TargetUrl = targetUrl,
            Positions = positions
        }
    );

    public Task<IEnumerable<TrendEntryEntity>> GetSearchTrendsAsync(
        string engineName,
        string queryText,
        string targetUrl
    ) => _context.QueryAsync<TrendEntryEntity>(
        storedProc: "ranking.GetSearchTrends",
        parameters: new
        {
            EngineName = engineName,
            QueryText = queryText,
            TargetUrl = targetUrl
        }
    );
}