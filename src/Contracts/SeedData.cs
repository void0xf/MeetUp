using System;
using System.Collections.Generic;

namespace Contracts;

public static class SeedData
{
    public static List<EventData> GetSeedEvents()
    {
        return new List<EventData>
        {
            // Upcoming events (starting in the future)
            new EventData
            {
                Id = Guid.NewGuid(),
                Title = "Annual Meetup",
                Description = "A gathering of all members for the annual meetup.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.UtcNow.AddMonths(2),
                EventEndDate = DateTime.UtcNow.AddMonths(2).AddHours(8),
                Location = "City Hall",
                Author = "Organizer Name",
                IsEnded = false,
                Participants = new List<string> { "Alice", "Bob" },
                Visibility = "Public",
                Images = new List<string> { "image1.jpg", "image2.jpg" }
            },
            new EventData
            {
                Id = Guid.NewGuid(),
                Title = "Tech Conference",
                Description = "An exciting tech conference with industry leaders.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.UtcNow.AddMonths(3),
                EventEndDate = DateTime.UtcNow.AddMonths(3).AddHours(8),
                Location = "Tech Hub",
                Author = "Tech Organizer",
                IsEnded = false,
                Participants = new List<string> { "Charlie", "Dana" },
                Visibility = "InviteOnly",
                Images = new List<string> { "tech1.jpg", "tech2.jpg" }
            },
            // Ending soon events (ending within next 24 hours)
            new EventData
            {
                Id = Guid.NewGuid(),
                Title = "Community Workshop",
                Description = "Learn new skills at our community workshop.",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2),
                EventStartDate = DateTime.UtcNow.AddHours(-3),
                EventEndDate = DateTime.UtcNow.AddHours(20),
                Location = "Community Center",
                Author = "Workshop Leader",
                IsEnded = false,
                Participants = new List<string> { "Grace", "Henry" },
                Visibility = "Public",
                Images = new List<string> { "workshop1.jpg", "workshop2.jpg" }
            },
            new EventData
            {
                Id = Guid.NewGuid(),
                Title = "Networking Mixer",
                Description = "Connect with professionals in your industry.",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1),
                EventStartDate = DateTime.UtcNow.AddHours(-2),
                EventEndDate = DateTime.UtcNow.AddHours(10),
                Location = "Downtown Hotel",
                Author = "Networking Group",
                IsEnded = false,
                Participants = new List<string> { "Ivan", "Julia" },
                Visibility = "OnRequest",
                Images = new List<string> { "mixer1.jpg", "mixer2.jpg" }
            },
            // Regular event
            new EventData
            {
                Id = Guid.NewGuid(),
                Title = "Community BBQ",
                Description = "A fun BBQ event for the local community.",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EventStartDate = DateTime.UtcNow.AddDays(15),
                EventEndDate = DateTime.UtcNow.AddDays(15).AddHours(3),
                Location = "Community Park",
                Author = "Community Leader",
                IsEnded = false,
                Participants = new List<string> { "Eve", "Frank" },
                Visibility = "OnRequest",
                Images = new List<string> { "bbq1.jpg", "bbq2.jpg" }
            }
        };
    }
}

public class EventData
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Location { get; set; }
    public string Author { get; set; }
    public bool IsEnded { get; set; }
    public List<string> Participants { get; set; }
    public string Visibility { get; set; }
    public string ConversationId { get; set; }
    public List<string> Images { get; set; }
} 