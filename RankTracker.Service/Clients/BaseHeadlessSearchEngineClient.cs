using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Extensions.Options;
using RankTracker.Service.Configuration;
using RankTracker.Service.Factories;

namespace RankTracker.Service.Clients
{
    public abstract class BaseHeadlessSearchEngineClient : ISearchEngineClient, IAsyncDisposable
    {
        private readonly IBrowserContextFactory _contextFactory;
        protected readonly SearchEngineOptions Options;

        protected BaseHeadlessSearchEngineClient(
            IBrowserContextFactory contextFactory,
            IOptions<SearchEngineOptions> options)
        {
            _contextFactory = contextFactory;
            Options = options.Value;
        }

        protected abstract string BuildSearchUrl(string query);

        protected string selector { get; set; }

        protected abstract List<(string Domain, int Position)> ExtractDomainPositions(string html);

        public async Task<List<(string Domain, int Position)>> GetResultsWithPositionsAsync(string query)
        {
            var context = await _contextFactory.GetContextAsync();
            var page = context.Pages.FirstOrDefault() ?? await context.NewPageAsync();

            var url = BuildSearchUrl(query);
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
            await page.WaitForSelectorAsync(selector);

            var html = await page.ContentAsync();
            return ExtractDomainPositions(html);
        }

        public async ValueTask DisposeAsync()
            => await _contextFactory.DisposeAsync();
    }
}