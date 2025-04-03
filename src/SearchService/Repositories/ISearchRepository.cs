using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Repositories;

public interface ISearchRepository
{
    Task<(List<Item> Results, int PageCount, int TotalCount)> SearchItemsAsync(RequestParams requestParams);
    Task<bool> SaveItemAsync(Item item);
    Task<bool> UpdateItemAsync(Item item);
    Task<bool> DeleteItemAsync(string id);
} 