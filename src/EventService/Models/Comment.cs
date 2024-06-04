using System.ComponentModel.DataAnnotations.Schema;

namespace EventService.Models;

[Table("Comments")]
public class Comment
{
    public Guid Id { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public MeetEvent MeetEvent { get; set; }
    public Guid MeetEventId { get; set; }
}
