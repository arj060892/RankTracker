using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankTracker.Application.Interfaces;
using RankTracker.Application.Models.Requests;
using RankTracker.Application.Models.Responses;

namespace RankTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrendsController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public TrendsController(ISearchService searchService)
            => _searchService = searchService;

        [HttpGet]
        public async Task<ActionResult<TrendEntryDto[]>> Get([FromQuery] SearchRequestDto request)
        {
            var trends = await _searchService.GetTrendsAsync(
                request.Engine, request.Query, request.Url);

            var dtoArray = trends.ToArray();
            return Ok(dtoArray);
        }
    }
}
