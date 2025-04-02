using System.ComponentModel.DataAnnotations;
using EventService.Models;

namespace EventService.DTOs;

public class CreateMeetEventDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public DateTime EventStartDate { get; set; }

    [Required]
    public DateTime EventEndDate { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public EventType Visibility { get; set; } = EventType.Public;

    [Required]
    public List<string> Images { get; set; }
}
