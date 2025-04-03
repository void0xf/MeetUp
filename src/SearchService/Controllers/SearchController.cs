using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] RequestParams requestParams)
    {
        var query = DB.PagedSearch<Item, Item>();
        query.Sort(x => x.Ascending(a => a.Title));

        if (!string.IsNullOrEmpty(requestParams.SearchTerm))
        {
            query.Match(Search.Full, requestParams.SearchTerm);
        }

        query = requestParams.OrderBy switch
        {
            "Title" => query.Sort(x => x.Ascending(a => a.Title)),
            _ => query.Sort(x => x.Ascending(a => a.Author))
        };

        if (string.IsNullOrEmpty(requestParams.FilterBy))
        {
            // No filter applied when FilterBy is null or empty
        }
        else
        {
            query = requestParams.FilterBy switch
            {
                "EndingSoon" => query.Match(e => e.EventEndDate < DateTime.UtcNow.AddDays(1)),
                "Upcoming" => query.Match(e => e.EventStartDate > DateTime.UtcNow),
                _ => query.Match(e => e.EventStartDate >= DateTime.UtcNow)
            };
        }

        query.PageNumber(requestParams.PageNumber);
        query.PageSize(requestParams.PageSize);

        var queryResult = await query.ExecuteAsync();

        return Ok(
            new
            {
                queryResult.Results,
                queryResult.PageCount,
                queryResult.TotalCount
            }
        );
    }
}
