using Microsoft.AspNetCore.Mvc;
using SearchService.Models;
using SearchService.RequestHelpers;
using SearchService.Services;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] RequestParams requestParams)
    {
        var result = await _searchService.SearchItemsAsync(requestParams);
        
        return Ok(
            new
            {
                result.Results,
                result.PageCount,
                result.TotalCount
            }
        );
    }
}
