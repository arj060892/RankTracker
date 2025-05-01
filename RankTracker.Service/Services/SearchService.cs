using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RankTracker.Application.Interfaces;
using RankTracker.Application.Models.Requests;
using RankTracker.Application.Models.Responses;
using RankTracker.Persistence.Repositories;
using RankTracker.Service.Clients;

namespace RankTracker.Service.Services
{
    public class SearchService : ISearchService
    {
        private readonly Func<string, ISearchEngineClient> _clientFactory;
        private readonly ISearchHistoryRepository _historyRepo;
        private readonly IMemoryCache _cache;

        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public SearchService(
            Func<string, ISearchEngineClient> clientFactory,
            ISearchHistoryRepository historyRepo,
            IMemoryCache cache)
        {
            _clientFactory = clientFactory;
            _historyRepo = historyRepo;
            _cache = cache;
        }

        public async Task<SearchResultDto> SearchAsync(SearchRequestDto request)
        {
            var client = _clientFactory(request.Engine);
            var cacheKey = $"{request.Engine}|{request.Query}|{request.Url}";

            if (!_cache.TryGetValue(cacheKey, out SearchResultDto result))
            {
                var domainPositions = await client.GetResultsWithPositionsAsync(request.Query);

                var positions = domainPositions
                    .Where(dp => dp.Domain.Contains(request.Url, StringComparison.OrdinalIgnoreCase))
                    .Select(dp => dp.Position)
                    .ToList();

                var csv = positions.Any()
                    ? string.Join(',', positions)
                    : "0";

                await _historyRepo.InsertSearchHistoryAsync(
                    request.Engine,
                    request.Query,
                    request.Url,
                    csv);

                result = new SearchResultDto(positions);
                _cache.Set(cacheKey, result, _cacheDuration);
            }

            return result;
        }


        public async Task<IEnumerable<TrendEntryDto>> GetTrendsAsync(
            string engine,
            string query,
            string url)
        {
            var entries = await _historyRepo.GetSearchTrendsAsync(engine, query, url);
            return entries.Select(e => new TrendEntryDto(
                e.CheckedAt,
                e.Positions
                 .Split(',', StringSplitOptions.RemoveEmptyEntries)
                 .Select(int.Parse)
                 .ToList()
            ));
        }
    }
}
