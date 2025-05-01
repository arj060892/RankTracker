using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using RankTracker.Service.Factories;

namespace RankTracker.Service.Services
{
    public class PlaywrightCookieInitializer : IHostedService
    {
        private readonly IBrowserContextFactory _factory;

        public PlaywrightCookieInitializer(IBrowserContextFactory factory)
            => _factory = factory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var context = await _factory.GetContextAsync();
            var page = context.Pages.Count > 0
                        ? context.Pages[0]
                        : await context.NewPageAsync();

            await page.GotoAsync("https://www.google.co.uk", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            var consent = page.Locator("button#L2AGLb, button[aria-label*='Agree']");
            if (await consent.CountAsync() > 0 && await consent.IsVisibleAsync())
            {
                await consent.ClickAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
