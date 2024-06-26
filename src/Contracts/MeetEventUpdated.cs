﻿namespace Contracts;

public class MeetEventUpdated
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Location { get; set; }
    public List<string> Participants { get; set; }
    public string ConversationId { get; set; }
    public string Visibility { get; set; }
    public List<string> Images { get; set; }
}
