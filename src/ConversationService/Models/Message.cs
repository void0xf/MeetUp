namespace ConversationService.Models;

public class Message
{
    public string Sender { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
