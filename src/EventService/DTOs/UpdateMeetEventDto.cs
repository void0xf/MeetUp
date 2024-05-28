using System.ComponentModel.DataAnnotations;

namespace EventService.DTOs;

public class UpdateMeetEventDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public DateTime EventStartDate { get; set; }

    [Required]
    public DateTime EventEndDate { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public string Visibility { get; set; }

    [Required]
    public List<string> Images { get; set; }
}
