using AutoMapper;
using EventService.DTOs;
using EventService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Controllers;

[ApiController]
[Route("api/MeetEvent")]
public class MeetEventController : ControllerBase
{
    private readonly IEventService _eventService;

    public MeetEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MeetEventDto>>> GetAllEvents()
    {
        var meetEvents = await _eventService.GetAllEventsAsync();
        return Ok(meetEvents);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MeetEventDto>> GetMeetEventById(Guid id)
    {
        var meetEvent = await _eventService.GetMeetEventByIdAsync(id);
        
        if (meetEvent == null)
            return NotFound();

        return Ok(meetEvent);
    }

    [HttpGet("me")]
    public async Task<ActionResult<List<MeetEventDto>>> GetMyMeetEvents()
    {
        var meetEvents = await _eventService.GetMyMeetEventsAsync(User.Identity.Name);

        if (meetEvents == null)
            return NotFound();

        return Ok(meetEvents);
    }

    [HttpPost("AddUserToParticipantList/{meetEventId}")]
    public async Task<ActionResult> AddUserToParticipantList(string meetEventId)
    {
        var result = await _eventService.AddUserToParticipantListAsync(meetEventId, User.Identity.Name);
        
        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<MeetEventDto>> CreateMeetEvent(
        CreateMeetEventDto createMeetEvent
    )
    {
        var createdEvent = await _eventService.CreateMeetEventAsync(createMeetEvent, User.Identity.Name);

        if (createdEvent == null)
            return BadRequest("Could not create meet event");

        return CreatedAtAction(
            nameof(GetMeetEventById),
            new { id = createdEvent.Id },
            createdEvent
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeetEvent(Guid id, UpdateMeetEventDto updateMeetEvent)
    {
        var result = await _eventService.UpdateMeetEventAsync(id, updateMeetEvent, User.Identity.Name);
        
        if (!result)
            return NotFound("Event not found or you don't have permission to update it");

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeetEvent(Guid id)
    {
        var result = await _eventService.DeleteMeetEventAsync(id, User.Identity.Name);
        
        if (!result)
            return NotFound("Event not found or you don't have permission to delete it");

        return Ok();
    }
}
