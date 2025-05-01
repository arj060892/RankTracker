using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankTracker.Application.Models.Responses;
using RankTracker.Persistence.Repositories;

namespace RankTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnginesController(ISearchEngineRepository engineRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<EngineDto[]>> GetEngines()
        {
            var entities = await engineRepo.GetAllAsync();
            var dtos = entities
                .Select(e => new EngineDto(e.EngineId, e.Name))
                .ToArray();
            return Ok(dtos);
        }
    }
}
