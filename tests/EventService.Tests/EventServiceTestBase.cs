using AutoMapper;
using Contracts;
using EventService.DTOs;
using EventService.Models;
using EventService.Repositories;
using EventService.Services;
using MassTransit;
using Moq;

namespace EventService.Tests;

public class EventServiceTestBase
{
    protected readonly Mock<IEventRepository> MockRepository;
    protected readonly Mock<IMapper> MockMapper;
    protected readonly Mock<IPublishEndpoint> MockPublishEndpoint;
    protected readonly EventService.Services.EventService EventService;

    protected EventServiceTestBase()
    {
        MockRepository = new Mock<IEventRepository>();
        MockMapper = new Mock<IMapper>();
        MockPublishEndpoint = new Mock<IPublishEndpoint>();

        EventService = new EventService.Services.EventService(
            MockRepository.Object,
            MockMapper.Object,
            MockPublishEndpoint.Object
        );
    }

    protected MeetEvent CreateTestMeetEvent(
        Guid id = default,
        string author = "testuser",
        string title = "Test Event",
        bool withParticipants = true,
        bool withConversationId = true
    )
    {
        return new MeetEvent
        {
            Id = id == default ? Guid.NewGuid() : id,
            Title = title,
            Description = "Test description",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            EventStartDate = DateTime.UtcNow.AddDays(1),
            EventEndDate = DateTime.UtcNow.AddDays(2),
            Location = "Test location",
            Author = author,
            IsEnded = false,
            Participants = withParticipants ? new List<string> { "participant1", "participant2" } : null,
            ConversationId = withConversationId ? Guid.NewGuid().ToString() : null,
            Visibility = EventType.Public,
            Images = new List<string> { "image1.jpg", "image2.jpg" },
            Comments = new List<Comment>()
        };
    }

    protected List<MeetEvent> CreateTestMeetEvents(int count = 3, string author = "testuser")
    {
        var result = new List<MeetEvent>();
        for (int i = 0; i < count; i++)
        {
            result.Add(CreateTestMeetEvent(Guid.NewGuid(), author: i % 2 == 0 ? author : $"user{i}"));
        }
        return result;
    }

    protected MeetEventDto CreateTestMeetEventDto(Guid id = default)
    {
        return new MeetEventDto
        {
            Id = id == default ? Guid.NewGuid() : id,
            Title = "Test Event DTO",
            Description = "Test description DTO",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            EventStartDate = DateTime.UtcNow.AddDays(1),
            EventEndDate = DateTime.UtcNow.AddDays(2),
            Location = "Test location DTO",
            Author = "testuser",
            Visibility = "Public",
            Participants = new List<string> { "participant1", "participant2" },
            ConversationId = Guid.NewGuid().ToString(),
            Images = new List<string> { "image1.jpg", "image2.jpg" }
        };
    }

    protected List<MeetEventDto> CreateTestMeetEventDtos(int count = 3)
    {
        var result = new List<MeetEventDto>();
        for (int i = 0; i < count; i++)
        {
            result.Add(CreateTestMeetEventDto(Guid.NewGuid()));
        }
        return result;
    }

    protected CreateMeetEventDto CreateTestCreateMeetEventDto()
    {
        return new CreateMeetEventDto
        {
            Title = "New Test Event",
            Description = "New Test description",
            EventStartDate = DateTime.UtcNow.AddDays(1),
            EventEndDate = DateTime.UtcNow.AddDays(2),
            Location = "New Test location",
            Visibility = EventType.Public,
            Images = new List<string> { "new-image1.jpg", "new-image2.jpg" }
        };
    }

    protected UpdateMeetEventDto CreateTestUpdateMeetEventDto()
    {
        return new UpdateMeetEventDto
        {
            Title = "Updated Test Event",
            Description = "Updated Test description",
            EventStartDate = DateTime.UtcNow.AddDays(3),
            EventEndDate = DateTime.UtcNow.AddDays(4),
            Location = "Updated Test location",
            Visibility = EventType.InviteOnly,
            Images = new List<string> { "updated-image1.jpg", "updated-image2.jpg" }
        };
    }
} 