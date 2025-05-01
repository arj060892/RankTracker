using RankTracker.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankTracker.Persistence.Repositories
{
    public interface ISearchEngineRepository
    {
        Task<IEnumerable<EngineEntity>> GetAllAsync();
    }
}