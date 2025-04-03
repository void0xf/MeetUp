using AutoMapper;
using Contracts;
using EventService.DTOs;
using EventService.Models;
using EventService.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public EventService(
        IEventRepository repository,
        IMapper mapper,
        IPublishEndpoint publishEndpoint
    )
    {
        _repository = repository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<List<MeetEventDto>> GetAllEventsAsync()
    {
        var meetEvents = await _repository.GetAllAsync();
        return _mapper.Map<List<MeetEventDto>>(meetEvents);
    }

    public async Task<MeetEventDto> GetMeetEventByIdAsync(Guid id)
    {
        var meetEvent = await _repository.GetByIdAsync(id);
        
        if (meetEvent == null)
            return null;
            
        return _mapper.Map<MeetEventDto>(meetEvent);
    }

    public async Task<List<MeetEventDto>> GetMyMeetEventsAsync(string username)
    {
        var meetEvents = await _repository.GetByAuthorAsync(username);
        
        if (meetEvents == null || meetEvents.Count == 0)
            return null;

        return _mapper.Map<List<MeetEventDto>>(meetEvents);
    }

    public async Task<bool> AddUserToParticipantListAsync(string meetEventId, string username)
    {
        var meetEventToUpdate = await _repository.GetByIdAsync(Guid.Parse(meetEventId));
        
        if (meetEventToUpdate == null)
            return false;
            
        if (meetEventToUpdate.Participants == null)
        {
            meetEventToUpdate.Participants = new List<string>();
        }
        
        // Check if user is already a participant
        if (!meetEventToUpdate.Participants.Contains(username))
        {
            meetEventToUpdate.Participants.Add(username);
            
            var message = new MeetEventParticipantAdded();
            message.ConversationId = meetEventToUpdate.ConversationId;
            message.ParticipantUsername = username;

            await _publishEndpoint.Publish(message);
            
            await _repository.UpdateAsync(meetEventToUpdate);
            return await _repository.SaveChangesAsync();
        }

        return true;
    }

    public async Task<MeetEventDto> CreateMeetEventAsync(CreateMeetEventDto createMeetEvent, string username)
    {
        var mappedMeetEvent = _mapper.Map<MeetEvent>(createMeetEvent);
        mappedMeetEvent.Author = username;

        await _repository.AddAsync(mappedMeetEvent);

        var meetEventMessage = _mapper.Map<MeetEventCreated>(mappedMeetEvent);
        await _publishEndpoint.Publish(meetEventMessage);

        var result = await _repository.SaveChangesAsync();

        if (!result)
            return null;

        return _mapper.Map<MeetEventDto>(mappedMeetEvent);
    }

    public async Task<bool> UpdateMeetEventAsync(Guid id, UpdateMeetEventDto updateMeetEvent, string username)
    {
        var meetEventToUpdate = await _repository.GetByIdAsync(id);
        
        if (meetEventToUpdate == null)
            return false;
            
        if (meetEventToUpdate.Author != username)
            return false;

        meetEventToUpdate.Title = updateMeetEvent.Title;
        meetEventToUpdate.Description = updateMeetEvent.Description;
        meetEventToUpdate.EventEndDate = updateMeetEvent.EventEndDate;
        meetEventToUpdate.EventStartDate = updateMeetEvent.EventStartDate;
        meetEventToUpdate.Images = updateMeetEvent.Images;
        meetEventToUpdate.Location = updateMeetEvent.Location;
        meetEventToUpdate.Visibility = updateMeetEvent.Visibility;
        meetEventToUpdate.UpdatedAt = DateTime.UtcNow;

        var meetEventMessage = _mapper.Map<MeetEventUpdated>(meetEventToUpdate);
        await _publishEndpoint.Publish(meetEventMessage);

        await _repository.UpdateAsync(meetEventToUpdate);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteMeetEventAsync(Guid id, string username)
    {
        var meetEvent = await _repository.GetByIdAsync(id);
        
        if (meetEvent == null)
            return false;

        if (meetEvent.Author != username)
            return false;

        var meetEventMessage = _mapper.Map<MeetEventDeleted>(meetEvent);
        await _publishEndpoint.Publish(meetEventMessage);

        await _repository.DeleteAsync(meetEvent);
        return await _repository.SaveChangesAsync();
    }
} 