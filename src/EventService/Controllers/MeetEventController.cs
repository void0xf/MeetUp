using AutoMapper;
using Contracts;
using EventService.Data;
using EventService.DTOs;
using EventService.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.Controllers;

[ApiController]
[Route("api/MeetEvent")]
public class MeetEventController : ControllerBase
{
    private readonly MeetEventDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public MeetEventController(
        MeetEventDbContext context,
        IMapper mapper,
        IPublishEndpoint publishEndpoint
    )
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }
    [HttpGet]
    public async Task<ActionResult<List<MeetEventDto>>> GetAllEvents()
    {
        var meetEvents = await _context.MeetEvents.ToListAsync<MeetEvent>();
        var dtoMeetEvents = _mapper.Map<List<MeetEventDto>>(meetEvents);
        return Ok(dtoMeetEvents);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<MeetEventDto>> GetMeetEventById(Guid id)
    {
        var MeetEvent = await _context.MeetEvents.FirstOrDefaultAsync(x => x.Id == id);
        
        if (MeetEvent == null)
            return NotFound();
            
        var dtoMeetEvent = _mapper.Map<MeetEventDto>(MeetEvent);

        return Ok(dtoMeetEvent);
    }

    [HttpGet("me")]
    public async Task<ActionResult<List<MeetEventDto>>> GetMyMeetEvents()
    {
        var meetEvents = await _context
            .MeetEvents.Where(x => x.Author == User.Identity.Name)
            .ToListAsync();
        var dtoMeetEvents = _mapper.Map<List<MeetEventDto>>(meetEvents);

        if (meetEvents == null || meetEvents.Count == 0)
            return NotFound();

        return Ok(dtoMeetEvents);
    }

    
    [HttpPost("AddUserToParticipantList/{meetEventId}")]
    public async Task<ActionResult> AddUserToParticipantList(string meetEventId)
    {
        var meetEventToUpdate = await _context.MeetEvents.FirstOrDefaultAsync(m =>
            m.Id.ToString() == meetEventId
        );
        if (meetEventToUpdate == null)
            return NotFound();

        var currentUsername = User.Identity.Name;
            
        if (meetEventToUpdate.Participants == null)
        {
            meetEventToUpdate.Participants = new List<string>();
        }
        
        // Check if user is already a participant
        if (!meetEventToUpdate.Participants.Contains(currentUsername))
        {
            meetEventToUpdate.Participants.Add(currentUsername);
            
            var message = new MeetEventParticipantAdded();
            message.ConversationId = meetEventToUpdate.ConversationId;
            message.ParticipantUsername = currentUsername;

            await _publishEndpoint.Publish(message);
            
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    
    [HttpPost]
    public async Task<ActionResult<CreateMeetEventDto>> CreateMeetEvent(
        CreateMeetEventDto createMeetEvent
    )
    {
        var MappedMeetEvent = _mapper.Map<MeetEvent>(createMeetEvent);

        MappedMeetEvent.Author = User.Identity.Name;

        _context.MeetEvents.Add(MappedMeetEvent);

        var meetEventMessage = _mapper.Map<MeetEventCreated>(MappedMeetEvent);

        await _publishEndpoint.Publish(meetEventMessage);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return BadRequest("Could not create meet event");

        return CreatedAtAction(
            nameof(GetMeetEventById),
            new { MappedMeetEvent.Id, },
            _mapper.Map<MeetEventDto>(MappedMeetEvent)
        );
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeetEvent(Guid id, UpdateMeetEventDto updateMeetEvent)
    {
        var MeetEventToUpdate = await _context.MeetEvents.FirstOrDefaultAsync(me =>
            me.Id == id
        );
        
        if (MeetEventToUpdate == null)
            return NotFound("Event not found");
            
        if (MeetEventToUpdate.Author != User.Identity.Name)
            return Forbid();

        MeetEventToUpdate.Title = updateMeetEvent.Title;
        MeetEventToUpdate.Description = updateMeetEvent.Description;
        MeetEventToUpdate.EventEndDate = updateMeetEvent.EventEndDate;
        MeetEventToUpdate.EventStartDate = updateMeetEvent.EventStartDate;
        MeetEventToUpdate.Images = updateMeetEvent.Images;
        MeetEventToUpdate.Location = updateMeetEvent.Location;
        MeetEventToUpdate.Visibility = updateMeetEvent.Visibility;
        MeetEventToUpdate.UpdatedAt = DateTime.UtcNow;

        var meetEventMessage = _mapper.Map<MeetEventUpdated>(MeetEventToUpdate);

        await _publishEndpoint.Publish(meetEventMessage);

        var result = await _context.SaveChangesAsync() > 0;
        if (result)
            return Ok();

        return BadRequest("Problem with saving changes");
    }

    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeetEvent(Guid id)
    {
        var MeetEvent = await _context.MeetEvents.FirstOrDefaultAsync(me => me.Id == id);
        
        if (MeetEvent == null)
            return NotFound("Event not found");

        if (MeetEvent.Author != User.Identity.Name)
            return Forbid();

        _context.MeetEvents.Remove(MeetEvent);

        var meetEventMessage = _mapper.Map<MeetEventDeleted>(MeetEvent);

        await _publishEndpoint.Publish(meetEventMessage);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return BadRequest("Could not save changes");

        return Ok();
    }
}
