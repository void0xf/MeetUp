using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Repositories;

public class SearchRepository : ISearchRepository
{
    public async Task<(List<Item> Results, int PageCount, int TotalCount)> SearchItemsAsync(RequestParams requestParams)
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

        if (!string.IsNullOrEmpty(requestParams.FilterBy))
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

        return (queryResult.Results.ToList(), queryResult.PageCount, (int)queryResult.TotalCount);
    }

    public async Task<bool> SaveItemAsync(Item item)
    {
        try
        {
            await item.SaveAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving item: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        try
        {
            if (string.IsNullOrEmpty(item.ID))
            {
                throw new ArgumentException("Item ID cannot be null or empty when updating");
            }
            
            await DB.Update<Item>()
                .Match(i => i.ID == item.ID)
                .ModifyWith(item)
                .ExecuteAsync();
                
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating item: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty when deleting");
            }
            
            await DB.DeleteAsync<Item>(id);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting item: {ex.Message}");
            return false;
        }
    }
} 