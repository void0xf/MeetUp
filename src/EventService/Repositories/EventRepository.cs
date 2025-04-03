using EventService.Data;
using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Repositories;

public class EventRepository : IEventRepository
{
    private readonly MeetEventDbContext _context;

    public EventRepository(MeetEventDbContext context)
    {
        _context = context;
    }

    public async Task<List<MeetEvent>> GetAllAsync()
    {
        return await _context.MeetEvents.ToListAsync();
    }

    public async Task<MeetEvent> GetByIdAsync(Guid id)
    {
        return await _context.MeetEvents.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<MeetEvent>> GetByAuthorAsync(string username)
    {
        return await _context.MeetEvents
            .Where(x => x.Author == username)
            .ToListAsync();
    }

    public async Task<MeetEvent> AddAsync(MeetEvent meetEvent)
    {
        await _context.MeetEvents.AddAsync(meetEvent);
        return meetEvent;
    }

    public async Task<bool> UpdateAsync(MeetEvent meetEvent)
    {
        _context.MeetEvents.Update(meetEvent);
        return true;
    }

    public async Task<bool> DeleteAsync(MeetEvent meetEvent)
    {
        _context.MeetEvents.Remove(meetEvent);
        return true;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
} 