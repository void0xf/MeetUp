using MongoDB.Entities;

namespace ConversationService.Models;

public class Conversation : Entity
{
    public bool IsDM { get; set; }
    public List<string> Participants { get; set; } = new List<string>();
    public List<Message> Messages { get; set; } = new List<Message>();
}

public class ConversationDTO
{
    public bool IsDM { get; set; }
    public List<string> Participants { get; set; } = new List<string>();
    public List<Message> Messages { get; set; } = new List<Message>();
}
