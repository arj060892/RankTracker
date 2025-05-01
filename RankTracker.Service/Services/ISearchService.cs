using RankTracker.Application.Models.Requests;
using RankTracker.Application.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankTracker.Application.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultDto> SearchAsync(SearchRequestDto request);

        Task<IEnumerable<TrendEntryDto>> GetTrendsAsync(string engine, string query, string url);
    }
}