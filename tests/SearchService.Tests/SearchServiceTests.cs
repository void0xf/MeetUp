using Moq;
using SearchService.Models;
using SearchService.RequestHelpers;
using Xunit;

namespace SearchService.Tests;

public class SearchServiceTests : SearchServiceTestBase
{
    [Fact]
    public async Task SearchItemsAsync_CallsRepositoryAndReturnsResult()
    {
        // Arrange
        var requestParams = CreateTestRequestParams(searchTerm: "event");
        var testItems = CreateTestItems();
        var expectedResult = (testItems, 1, 3);
        
        MockRepository.Setup(repo => repo.SearchItemsAsync(requestParams))
            .ReturnsAsync(expectedResult);
            
        // Act
        var result = await SearchService.SearchItemsAsync(requestParams);
        
        // Assert
        Assert.Equal(expectedResult.Item1, result.Results);
        Assert.Equal(expectedResult.Item2, result.PageCount);
        Assert.Equal(expectedResult.Item3, result.TotalCount);
        MockRepository.Verify(repo => repo.SearchItemsAsync(requestParams), Times.Once);
    }
    
    [Fact]
    public async Task CreateItemAsync_CallsRepositorySaveMethod()
    {
        // Arrange
        var item = CreateTestItem();
        
        MockRepository.Setup(repo => repo.SaveItemAsync(item))
            .ReturnsAsync(true);
            
        // Act
        var result = await SearchService.CreateItemAsync(item);
        
        // Assert
        Assert.True(result);
        MockRepository.Verify(repo => repo.SaveItemAsync(item), Times.Once);
    }
    
    [Fact]
    public async Task UpdateItemAsync_CallsRepositoryUpdateMethod()
    {
        // Arrange
        var item = CreateTestItem();
        
        MockRepository.Setup(repo => repo.UpdateItemAsync(item))
            .ReturnsAsync(true);
            
        // Act
        var result = await SearchService.UpdateItemAsync(item);
        
        // Assert
        Assert.True(result);
        MockRepository.Verify(repo => repo.UpdateItemAsync(item), Times.Once);
    }
    
    [Fact]
    public async Task DeleteItemAsync_CallsRepositoryDeleteMethod()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        MockRepository.Setup(repo => repo.DeleteItemAsync(id))
            .ReturnsAsync(true);
            
        // Act
        var result = await SearchService.DeleteItemAsync(id);
        
        // Assert
        Assert.True(result);
        MockRepository.Verify(repo => repo.DeleteItemAsync(id), Times.Once);
    }
} 