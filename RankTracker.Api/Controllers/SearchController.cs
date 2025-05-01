using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankTracker.Application.Interfaces;
using RankTracker.Application.Models.Requests;
using RankTracker.Application.Models.Responses;

namespace RankTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
            => _searchService = searchService;

        [HttpPost]
        public async Task<ActionResult<SearchResultDto>> Post([FromBody] SearchRequestDto request)
        {
            var result = await _searchService.SearchAsync(request);
            return Ok(result);
        }
    }
}
