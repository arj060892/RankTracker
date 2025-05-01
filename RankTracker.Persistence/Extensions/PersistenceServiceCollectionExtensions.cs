using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RankTracker.Persistence.Db;
using RankTracker.Persistence.Repositories;

namespace RankTracker.Persistence.Extensions;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new SqlConnectionFactory(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IDapperContext, DapperContext>();
        services.AddScoped<ISearchEngineRepository, SearchEngineRepository>();
        services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
        return services;
    }
}
