using EventService.Models;

namespace EventService.Repositories;

public interface IEventRepository
{
    Task<List<MeetEvent>> GetAllAsync();
    Task<MeetEvent> GetByIdAsync(Guid id);
    Task<List<MeetEvent>> GetByAuthorAsync(string username);
    Task<MeetEvent> AddAsync(MeetEvent meetEvent);
    Task<bool> UpdateAsync(MeetEvent meetEvent);
    Task<bool> DeleteAsync(MeetEvent meetEvent);
    Task<bool> SaveChangesAsync();
} 