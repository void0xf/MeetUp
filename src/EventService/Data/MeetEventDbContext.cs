using EventService.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data;

public class MeetEventDbContext : DbContext
{
    public MeetEventDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<MeetEvent> MeetEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
