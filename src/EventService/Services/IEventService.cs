using EventService.DTOs;
using EventService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Services;

public interface IEventService
{
    Task<List<MeetEventDto>> GetAllEventsAsync();
    Task<MeetEventDto> GetMeetEventByIdAsync(Guid id);
    Task<List<MeetEventDto>> GetMyMeetEventsAsync(string username);
    Task<bool> AddUserToParticipantListAsync(string meetEventId, string username);
    Task<MeetEventDto> CreateMeetEventAsync(CreateMeetEventDto createMeetEvent, string username);
    Task<bool> UpdateMeetEventAsync(Guid id, UpdateMeetEventDto updateMeetEvent, string username);
    Task<bool> DeleteMeetEventAsync(Guid id, string username);
} 