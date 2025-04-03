using Contracts;
using Moq;
using Xunit;

namespace EventService.Tests;

public class ParticipantManagementTests : EventServiceTestBase
{
    [Fact]
    public async Task AddUserToParticipantListAsync_WithValidIdAndNewUser_AddsParticipantAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var meetEventId = id.ToString();
        var username = "newparticipant";
        var meetEvent = CreateTestMeetEvent(id);
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        MockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<EventService.Models.MeetEvent>()))
            .ReturnsAsync(true);
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(true);
            
        // Act
        var result = await EventService.AddUserToParticipantListAsync(meetEventId, username);
        
        // Assert
        Assert.True(result);
        Assert.Contains(username, meetEvent.Participants);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.UpdateAsync(meetEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(
                It.Is<MeetEventParticipantAdded>(msg => 
                    msg.ConversationId == meetEvent.ConversationId && 
                    msg.ParticipantUsername == username
                ),
                default
            ),
            Times.Once
        );
    }
    
    [Fact]
    public async Task AddUserToParticipantListAsync_WithValidIdAndExistingUser_DoesNotAddParticipantAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var meetEventId = id.ToString();
        var username = "participant1"; // Already exists in the test event
        var meetEvent = CreateTestMeetEvent(id);
        var initialParticipantCount = meetEvent.Participants.Count;
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        // Act
        var result = await EventService.AddUserToParticipantListAsync(meetEventId, username);
        
        // Assert
        Assert.True(result);
        Assert.Equal(initialParticipantCount, meetEvent.Participants.Count); // No change in participants count
        Assert.Contains(username, meetEvent.Participants);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<EventService.Models.MeetEvent>()), Times.Never);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventParticipantAdded>(), default),
            Times.Never
        );
    }
    
    [Fact]
    public async Task AddUserToParticipantListAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var meetEventId = id.ToString();
        var username = "newparticipant";
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((EventService.Models.MeetEvent)null);
            
        // Act
        var result = await EventService.AddUserToParticipantListAsync(meetEventId, username);
        
        // Assert
        Assert.False(result);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<EventService.Models.MeetEvent>()), Times.Never);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        MockPublishEndpoint.Verify(
            pub => pub.Publish(It.IsAny<MeetEventParticipantAdded>(), default),
            Times.Never
        );
    }
    
    [Fact]
    public async Task AddUserToParticipantListAsync_WithNullParticipantsList_InitializesListAndAddsParticipant()
    {
        // Arrange
        var id = Guid.NewGuid();
        var meetEventId = id.ToString();
        var username = "newparticipant";
        var meetEvent = CreateTestMeetEvent(id, withParticipants: false); // No participants list
        
        Assert.Null(meetEvent.Participants); // Confirm it starts null
        
        MockRepository.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(meetEvent);
            
        MockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<EventService.Models.MeetEvent>()))
            .ReturnsAsync(true);
            
        MockRepository.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(true);
            
        // Act
        var result = await EventService.AddUserToParticipantListAsync(meetEventId, username);
        
        // Assert
        Assert.True(result);
        Assert.NotNull(meetEvent.Participants);
        Assert.Single(meetEvent.Participants);
        Assert.Contains(username, meetEvent.Participants);
        MockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        MockRepository.Verify(repo => repo.UpdateAsync(meetEvent), Times.Once);
        MockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
} 