using EventService.Models;
using Moq;
using Xunit;

namespace EventService.Tests;

public class GetEventsTests : EventServiceTestBase
{
    [Fact]
    public async Task GetAllEventsAsync_ReturnsAllMappedEvents()
    {
        // Arrange
        var meetEvents = CreateTestMeetEvents();
        var meetEventDtos = CreateTestMeetEventDtos();
        
        MockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(meetEvents);
            
        MockMapper.Setup(mapper => mapper.Map<List<EventService.DTOs.MeetEventDto>>(meetEvents))
            .Returns(meetEventDtos);
            
        // Act
        var result = await EventService.GetAllEventsAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(meetEventDtos.Count, result.Count);
        MockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        MockMapper.Verify(mapper => mapper.Map<List<EventService.DTOs.MeetEventDto>>(meetEvents), Times.Once);
    }
    
    [Fact]
    public async Task GetMeetEventByIdAsync_WithValidId_ReturnsMappedEvent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var meetEvent = CreateTestMeetEvent(id);
        var meetEventDto = CreateTestMeetEventDto(id);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        MockMapper.Setup(mapper => mapper.Map<EventService.DTOs.MeetEventDto>(meetEvent))
            .Returns(meetEventDto);
            
        // Act
        var result = await EventService.GetMeetEventByIdAsync(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockMapper.Verify(mapper => mapper.Map<EventService.DTOs.MeetEventDto>(meetEvent), Times.Once);
    }
    
    [Fact]
    public async Task GetMeetEventByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        MeetEvent nullMeetEvent = null;
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(nullMeetEvent);
            
        // Act
        var result = await EventService.GetMeetEventByIdAsync(id);
        
        // Assert
        Assert.Null(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockMapper.Verify(mapper => mapper.Map<EventService.DTOs.MeetEventDto>(It.IsAny<MeetEvent>()), Times.Never);
    }
    
    [Fact]
    public async Task GetMyMeetEventsAsync_WithValidUsername_ReturnsMappedEvents()
    {
        // Arrange
        var username = "testuser";
        var meetEvents = CreateTestMeetEvents(3, username);
        var meetEventDtos = CreateTestMeetEventDtos();
        
        MockRepository.Setup(repo => repo.GetByAuthorAsync(username))
            .ReturnsAsync(meetEvents);
            
        MockMapper.Setup(mapper => mapper.Map<List<EventService.DTOs.MeetEventDto>>(meetEvents))
            .Returns(meetEventDtos);
            
        // Act
        var result = await EventService.GetMyMeetEventsAsync(username);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(meetEventDtos.Count, result.Count);
        MockRepository.Verify(repo => repo.GetByAuthorAsync(username), Times.Once);
        MockMapper.Verify(mapper => mapper.Map<List<EventService.DTOs.MeetEventDto>>(meetEvents), Times.Once);
    }
    
    [Fact]
    public async Task GetMyMeetEventsAsync_WithNoEvents_ReturnsNull()
    {
        // Arrange
        var username = "testuser";
        List<MeetEvent> emptyList = new List<MeetEvent>();
        
        MockRepository.Setup(repo => repo.GetByAuthorAsync(username))
            .ReturnsAsync(emptyList);
            
        // Act
        var result = await EventService.GetMyMeetEventsAsync(username);
        
        // Assert
        Assert.Null(result);
        MockRepository.Verify(repo => repo.GetByAuthorAsync(username), Times.Once);
        MockMapper.Verify(
            mapper => mapper.Map<List<EventService.DTOs.MeetEventDto>>(It.IsAny<List<MeetEvent>>()), 
            Times.Never
        );
    }
} 