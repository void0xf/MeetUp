using Contracts;
using EventService.DTOs;
using EventService.Models;
using Moq;
using Xunit;

namespace EventService.Tests;

public class EventCrudTests : EventServiceTestBase
{
    [Fact]
    public async Task CreateMeetEventAsync_WithValidData_CreatesEventAndReturnsDto()
    {
        // Arrange
        var createDto = CreateTestCreateMeetEventDto();
        var mappedEvent = CreateTestMeetEvent();
        var username = "testuser";
        var resultDto = CreateTestMeetEventDto();
        
        MockMapper.Setup(mapper => mapper.Map<MeetEvent>(createDto))
            .Returns(mappedEvent);
            
        MockRepository.Setup(repo => repo.AddAsync(It.IsAny<MeetEvent>()))
            .ReturnsAsync(mappedEvent);
            
        MockMapper.Setup(mapper => mapper.Map<MeetEventCreated>(It.IsAny<MeetEvent>()))
            .Returns(new MeetEventCreated());
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(true);
            
        MockMapper.Setup(mapper => mapper.Map<MeetEventDto>(It.IsAny<MeetEvent>()))
            .Returns(resultDto);
            
        // Act
        var result = await EventService.CreateMeetEventAsync(createDto, username);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(resultDto, result);
        Assert.Equal(username, mappedEvent.Author);
        MockRepository.Verify(repo => repo.AddAsync(mappedEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventCreated>(), default),
            Times.Once
        );
    }
    
    [Fact]
    public async Task CreateMeetEventAsync_WhenSaveFails_ReturnsNull()
    {
        // Arrange
        var createDto = CreateTestCreateMeetEventDto();
        var mappedEvent = CreateTestMeetEvent();
        var username = "testuser";
        
        MockMapper.Setup(mapper => mapper.Map<MeetEvent>(createDto))
            .Returns(mappedEvent);
            
        MockRepository.Setup(repo => repo.AddAsync(It.IsAny<MeetEvent>()))
            .ReturnsAsync(mappedEvent);
            
        MockMapper.Setup(mapper => mapper.Map<MeetEventCreated>(It.IsAny<MeetEvent>()))
            .Returns(new MeetEventCreated());
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(false); // Save fails
            
        // Act
        var result = await EventService.CreateMeetEventAsync(createDto, username);
        
        // Assert
        Assert.Null(result);
        MockRepository.Verify(repo => repo.AddAsync(mappedEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventCreated>(), default),
            Times.Once
        );
        MockMapper.Verify(
            mapper => mapper.Map<MeetEventDto>(It.IsAny<MeetEvent>()),
            Times.Never
        );
    }
    
    [Fact]
    public async Task UpdateMeetEventAsync_WithValidIdAndAuthor_UpdatesEventAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = CreateTestUpdateMeetEventDto();
        var username = "testuser";
        var meetEvent = CreateTestMeetEvent(id, author: username);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        MockMapper.Setup(mapper => mapper.Map<MeetEventUpdated>(It.IsAny<MeetEvent>()))
            .Returns(new MeetEventUpdated());
            
        MockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<MeetEvent>()))
            .ReturnsAsync(true);
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(true);
            
        // Act
        var result = await EventService.UpdateMeetEventAsync(id, updateDto, username);
        
        // Assert
        Assert.True(result);
        Assert.Equal(updateDto.Title, meetEvent.Title);
        Assert.Equal(updateDto.Description, meetEvent.Description);
        Assert.Equal(updateDto.EventStartDate, meetEvent.EventStartDate);
        Assert.Equal(updateDto.EventEndDate, meetEvent.EventEndDate);
        Assert.Equal(updateDto.Location, meetEvent.Location);
        Assert.Equal(updateDto.Visibility, meetEvent.Visibility);
        Assert.Equal(updateDto.Images, meetEvent.Images);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.UpdateAsync(meetEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventUpdated>(), default),
            Times.Once
        );
    }
    
    [Fact]
    public async Task UpdateMeetEventAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = CreateTestUpdateMeetEventDto();
        var username = "testuser";
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((MeetEvent)null);
            
        // Act
        var result = await EventService.UpdateMeetEventAsync(id, updateDto, username);
        
        // Assert
        Assert.False(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<MeetEvent>()),
            Times.Never
        );
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventUpdated>(), default),
            Times.Never
        );
    }
    
    [Fact]
    public async Task UpdateMeetEventAsync_WithDifferentAuthor_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = CreateTestUpdateMeetEventDto();
        var username = "testuser";
        var eventAuthor = "differentuser";
        var meetEvent = CreateTestMeetEvent(id, author: eventAuthor);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        // Act
        var result = await EventService.UpdateMeetEventAsync(id, updateDto, username);
        
        // Assert
        Assert.False(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(
            repo => repo.UpdateAsync(It.IsAny<MeetEvent>()),
            Times.Never
        );
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventUpdated>(), default),
            Times.Never
        );
    }
    
    [Fact]
    public async Task DeleteMeetEventAsync_WithValidIdAndAuthor_DeletesEventAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testuser";
        var meetEvent = CreateTestMeetEvent(id, author: username);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        MockMapper.Setup(mapper => mapper.Map<MeetEventDeleted>(It.IsAny<MeetEvent>()))
            .Returns(new MeetEventDeleted());
            
        MockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<MeetEvent>()))
            .ReturnsAsync(true);
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(true);
            
        // Act
        var result = await EventService.DeleteMeetEventAsync(id, username);
        
        // Assert
        Assert.True(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.DeleteAsync(meetEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventDeleted>(), default),
            Times.Once
        );
    }
    
    [Fact]
    public async Task DeleteMeetEventAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testuser";
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((MeetEvent)null);
            
        // Act
        var result = await EventService.DeleteMeetEventAsync(id, username);
        
        // Assert
        Assert.False(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(
            repo => repo.DeleteAsync(It.IsAny<MeetEvent>()),
            Times.Never
        );
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventDeleted>(), default),
            Times.Never
        );
    }
    
    [Fact]
    public async Task DeleteMeetEventAsync_WithDifferentAuthor_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var username = "testuser";
        var eventAuthor = "differentuser";
        var meetEvent = CreateTestMeetEvent(id, author: eventAuthor);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        // Act
        var result = await EventService.DeleteMeetEventAsync(id, username);
        
        // Assert
        Assert.False(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(
            repo => repo.DeleteAsync(It.IsAny<MeetEvent>()),
            Times.Never
        );
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventDeleted>(), default),
            Times.Never
        );
    }
} 