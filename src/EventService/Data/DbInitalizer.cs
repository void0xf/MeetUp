using EventService.Models;
using Microsoft.EntityFrameworkCore;
using Contracts;

namespace EventService.Data;

public class DbInitalizer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetService<MeetEventDbContext>());
    }

    private static void SeedData(MeetEventDbContext dbContext)
    {
        dbContext.Database.Migrate();

        if (dbContext.MeetEvents.Any())
            return;

        // Seed data directly from shared SeedData class
        // Note: We're not publishing RabbitMQ messages for these seed events
        // Both EventService and SearchService should initialize with same seed data independently
        var seedEvents = Contracts.SeedData.GetSeedEvents();
        var events = seedEvents.Select(e => new MeetEvent
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            EventStartDate = e.EventStartDate,
            EventEndDate = e.EventEndDate,
            Location = e.Location,
            Author = e.Author,
            IsEnded = e.IsEnded,
            Participants = e.Participants,
            Visibility = Enum.Parse<EventType>(e.Visibility),
            ConversationId = e.ConversationId,
            Images = e.Images
        }).ToList();

        dbContext.AddRange(events);
        dbContext.SaveChanges();
        
        Console.WriteLine($"Seeded {events.Count} events to EventService database");
    }
}
