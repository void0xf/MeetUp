using SearchService.Models;
using SearchService.Repositories;
using SearchService.RequestHelpers;

namespace SearchService.Services;

public class SearchService : ISearchService
{
    private readonly ISearchRepository _repository;

    public SearchService(ISearchRepository repository)
    {
        _repository = repository;
    }

    public async Task<(List<Item> Results, int PageCount, int TotalCount)> SearchItemsAsync(RequestParams requestParams)
    {
        return await _repository.SearchItemsAsync(requestParams);
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        return await _repository.SaveItemAsync(item);
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        return await _repository.UpdateItemAsync(item);
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        return await _repository.DeleteItemAsync(id);
    }
} 