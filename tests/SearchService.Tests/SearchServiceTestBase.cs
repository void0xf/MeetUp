using Moq;
using SearchService.Models;
using SearchService.Repositories;
using SearchService.RequestHelpers;

namespace SearchService.Tests;

public class SearchServiceTestBase
{
    protected readonly Mock<ISearchRepository> MockRepository;
    protected readonly Services.SearchService SearchService;

    protected SearchServiceTestBase()
    {
        MockRepository = new Mock<ISearchRepository>();
        SearchService = new Services.SearchService(MockRepository.Object);
    }

    protected Item CreateTestItem(
        string id = null,
        string author = "testuser",
        string title = "Test Item"
    )
    {
        return new Item
        {
            ID = id ?? Guid.NewGuid().ToString(),
            Title = title,
            Description = "Test description",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            EventStartDate = DateTime.UtcNow.AddDays(1),
            EventEndDate = DateTime.UtcNow.AddDays(2),
            Location = "Test location",
            Author = author,
            Visibility = "Public",
            Images = new List<string> { "image1.jpg", "image2.jpg" }
        };
    }

    protected List<Item> CreateTestItems(int count = 3, string author = "testuser")
    {
        var result = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            result.Add(CreateTestItem(Guid.NewGuid().ToString(), author: i % 2 == 0 ? author : $"user{i}"));
        }
        return result;
    }

    protected RequestParams CreateTestRequestParams(
        string searchTerm = null,
        int pageNumber = 1,
        int pageSize = 10,
        string orderBy = null,
        string filterBy = null
    )
    {
        return new RequestParams
        {
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderBy = orderBy,
            FilterBy = filterBy
        };
    }
} 