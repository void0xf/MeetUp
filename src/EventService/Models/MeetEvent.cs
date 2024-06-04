namespace EventService.Models;

public class MeetEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Location { get; set; }
    public string Author { get; set; }
    public bool IsEnded { get; set; }
    public List<string> Participants { get; set; }
    public string ConversationId { get; set; }
    public EventType Visibility { get; set; }
    public List<string> Images { get; set; }
    public List<Comment> Comments { get; set; }
}
