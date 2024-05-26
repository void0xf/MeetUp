using EventService.Models;
using Microsoft.EntityFrameworkCore;

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

        var events = new List<MeetEvent>()
        {
            new MeetEvent
            {
                Id = Guid.NewGuid(),
                Title = "Annual Meetup",
                Description = "A gathering of all members for the annual meetup.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.SpecifyKind(
                    new DateTime(2024, 6, 1, 10, 0, 0),
                    DateTimeKind.Utc
                ),
                EventEndDate = DateTime.SpecifyKind(
                    new DateTime(2024, 6, 1, 18, 0, 0),
                    DateTimeKind.Utc
                ),
                Location = "City Hall",
                Author = "Organizer Name",
                IsEnded = false,
                Participants = new List<string> { "Alice", "Bob" },
                Visibility = EventType.Public,
                Images = new List<string> { "image1.jpg", "image2.jpg" }
            },
            new MeetEvent
            {
                Id = Guid.NewGuid(),
                Title = "Tech Conference",
                Description = "An exciting tech conference with industry leaders.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.SpecifyKind(
                    new DateTime(2024, 7, 15, 9, 0, 0),
                    DateTimeKind.Utc
                ),
                EventEndDate = DateTime.SpecifyKind(
                    new DateTime(2024, 7, 15, 17, 0, 0),
                    DateTimeKind.Utc
                ),
                Location = "Tech Hub",
                Author = "Tech Organizer",
                IsEnded = false,
                Participants = new List<string> { "Charlie", "Dana" },
                Visibility = EventType.InviteOnly,
                Images = new List<string> { "tech1.jpg", "tech2.jpg" }
            },
            new MeetEvent
            {
                Id = Guid.NewGuid(),
                Title = "Community BBQ",
                Description = "A fun BBQ event for the local community.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.SpecifyKind(
                    new DateTime(2024, 8, 10, 12, 0, 0),
                    DateTimeKind.Utc
                ),
                EventEndDate = DateTime.SpecifyKind(
                    new DateTime(2024, 8, 10, 15, 0, 0),
                    DateTimeKind.Utc
                ),
                Location = "Community Park",
                Author = "Community Leader",
                IsEnded = false,
                Participants = new List<string> { "Eve", "Frank" },
                Visibility = EventType.OnRequest,
                Images = new List<string> { "bbq1.jpg", "bbq2.jpg" }
            }
        };

        dbContext.AddRange(events);

        dbContext.SaveChanges();
    }
}
