using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace RankTracker.Service.Factories
{
    public interface IBrowserContextFactory : IAsyncDisposable
    {
        Task<IBrowserContext> GetContextAsync();
    }

    public class PlaywrightContextFactory : IBrowserContextFactory
    {
        private readonly Lazy<Task<IPlaywright>> _playwright;
        private readonly Lazy<Task<IBrowserContext>> _context;

        public PlaywrightContextFactory()
        {
            _playwright = new Lazy<Task<IPlaywright>>(Playwright.CreateAsync);
            _context = new Lazy<Task<IBrowserContext>>(async () =>
            {
                var pw = await _playwright.Value;
                return await pw.Chromium.LaunchPersistentContextAsync(
                    userDataDir: "playwright-user-data",
                    options: new BrowserTypeLaunchPersistentContextOptions
                    {
                        Headless = false,
                    });
            });
        }

        public Task<IBrowserContext> GetContextAsync() => _context.Value;

        public async ValueTask DisposeAsync()
        {
            if (_context.IsValueCreated)
            {
                var ctx = await _context.Value;
                await ctx.CloseAsync();
            }

            if (_playwright.IsValueCreated) (await _playwright.Value).Dispose();
        }
    }
}
