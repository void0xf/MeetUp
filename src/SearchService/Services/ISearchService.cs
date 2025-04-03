using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Services;

public interface ISearchService
{
    Task<(List<Item> Results, int PageCount, int TotalCount)> SearchItemsAsync(RequestParams requestParams);
    Task<bool> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(Item item);
    Task<bool> DeleteItemAsync(string id);
} 