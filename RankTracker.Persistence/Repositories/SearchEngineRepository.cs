using System.Collections.Generic;
using System.Threading.Tasks;
using RankTracker.Persistence.Db;
using RankTracker.Persistence.Models;

namespace RankTracker.Persistence.Repositories;

public class SearchEngineRepository : ISearchEngineRepository
{
    private readonly IDapperContext _context;

    public SearchEngineRepository(IDapperContext context)
        => _context = context;

    public Task<IEnumerable<EngineEntity>> GetAllAsync()
        => _context.QueryAsync<EngineEntity>(
            storedProc: "config.GetSearchEngines"
        );
}
