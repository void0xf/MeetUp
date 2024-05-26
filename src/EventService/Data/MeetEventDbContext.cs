using EventService.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data;

public class MeetEventDbContext : DbContext
{
    public MeetEventDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<MeetEvent> MeetEvents { get; set; }
}
