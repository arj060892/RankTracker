using RankTracker.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankTracker.Persistence.Repositories
{
    public interface ISearchHistoryRepository
    {
        Task<int> InsertSearchHistoryAsync(string engineName, string queryText, string targetUrl, string positions);

        Task<IEnumerable<TrendEntryEntity>> GetSearchTrendsAsync(string engineName, string queryText, string targetUrl);
    }
}