using AutoMapper;
using Contracts;
using EventService.Data;
using EventService.DTOs;
using EventService.Models;
using MassTransit;
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

        return Ok(MeetEvent);
    }

    [HttpPost]
    public async Task<ActionResult<CreateMeetEventDto>> CreateMeetEvent(
        CreateMeetEventDto createMeetEvent
    )
    {
        var MappedMeetEvent = _mapper.Map<MeetEvent>(createMeetEvent);

        MappedMeetEvent.Author = "Test";

        _context.MeetEvents.Add(MappedMeetEvent);

        var result = await _context.SaveChangesAsync() > 0;

        var meetEventMessage = _mapper.Map<MeetEventCreated>(MappedMeetEvent);

        await _publishEndpoint.Publish(meetEventMessage);

        if (!result)
            return BadRequest("Could not create meet event");

        return CreatedAtAction(
            nameof(GetMeetEventById),
            new { MappedMeetEvent.Id, },
            _mapper.Map<MeetEventDto>(MappedMeetEvent)
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMeetEvent(UpdateMeetEventDto updateMeetEvent)
    {
        var MeetEventToUpdate = await _context.MeetEvents.FirstOrDefaultAsync(me =>
            me.Id == updateMeetEvent.Id
        );
        if (MeetEventToUpdate == null)
            return BadRequest();

        MeetEventToUpdate.Title = updateMeetEvent.Title;
        MeetEventToUpdate.Description = updateMeetEvent.Description;
        MeetEventToUpdate.EventEndDate = updateMeetEvent.EventEndDate;
        MeetEventToUpdate.EventStartDate = updateMeetEvent.EventStartDate;
        MeetEventToUpdate.Images = updateMeetEvent.Images;
        MeetEventToUpdate.Location = updateMeetEvent.Location;

        var result = await _context.SaveChangesAsync() > 0;
        if (result)
            return Ok();

        return BadRequest("Problem with saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeetEvent(Guid id)
    {
        var MeetEvent = await _context.MeetEvents.FirstAsync(me => me.Id == id);
        if (MeetEvent == null)
            return NotFound("");

        _context.MeetEvents.Remove(MeetEvent);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return BadRequest("Could not save changes");

        return Ok();
    }
}
