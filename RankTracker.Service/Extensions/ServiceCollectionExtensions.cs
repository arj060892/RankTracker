using Microsoft.Extensions.DependencyInjection;
using RankTracker.Service.Factories;
using RankTracker.Service.Clients;
using RankTracker.Service.Configuration;
using RankTracker.Application.Interfaces;
using RankTracker.Service.Services;
using Microsoft.Extensions.Configuration;

namespace RankTracker.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<SearchEngineOptions>(
                config.GetSection("SearchEngine"));

            services.AddMemoryCache();

            services.AddSingleton<IBrowserContextFactory, PlaywrightContextFactory>();
            services.AddSingleton<GoogleHeadlessSearchEngineClient>();
            services.AddSingleton<BingHeadlessSearchEngineClient>();

            services.AddScoped<Func<string, ISearchEngineClient>>(sp => engine =>
              engine.ToLower() switch {
                  "google" => sp.GetRequiredService<GoogleHeadlessSearchEngineClient>(),
                  "bing" => sp.GetRequiredService<BingHeadlessSearchEngineClient>(),
                  _ => throw new KeyNotFoundException()
              });


            services.AddScoped<ISearchService, SearchService>();

            services.AddHostedService<PlaywrightCookieInitializer>();

            return services;
        }
    }
}
