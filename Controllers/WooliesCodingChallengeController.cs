using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Woolies.Model;
using Woolies.Model.Exception;
using Woolies.Services;

namespace Woolies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WooliesCodingChallengeController : ControllerBase
    {

        private readonly IOptions<WooliesXConfig> _wooliesXConfig;
        private readonly IResourceService _resourceService;
        private readonly ICatalogService _catalogService;

        public WooliesCodingChallengeController(IOptions<WooliesXConfig> wooliesXConfig, IResourceService resourceService, ICatalogService catalogService)
        {
            _wooliesXConfig = wooliesXConfig;
            _resourceService = resourceService;
            _catalogService = catalogService;
        }

        [HttpGet("user", Name = nameof(UserInfo))]
        public async Task<IActionResult> UserInfo()
        {
            var user = _wooliesXConfig.Value.User;
            var response = 
                await Task.FromResult(user);
            
            return Ok(response);
        }

        [HttpGet("sort", Name = nameof(Sort))]
        public async Task<IActionResult> Sort(string sortOption)
        {
            var sortByEnum = GetSortBy(sortOption);
            var sortedProducts = await _catalogService.GetSortedProducts(sortByEnum); 
            return Ok(sortedProducts);
        }

        [HttpPost("trolleyTotal")]
        public async Task<IActionResult> TrolleyTotal([FromBody]TrolleyCalculatorRequest request)
        {
            var total = await _resourceService.GetTrolleyTotal(request);
            return Ok(total);
        }

        // just a simple extractor with validation for the purpose of this exercise
        private static SortBy GetSortBy(string sortBy)
        {
            return sortBy?.ToLowerInvariant() switch
            {
                "low" => SortBy.Low,
                "high" => SortBy.High,
                "ascending" => SortBy.Ascending,
                "descending" => SortBy.Descending,
                "recommended" => SortBy.Recommended,
                _ => throw new ValidationException("Invalid sortBy parameter")
            };
        }
    }
}