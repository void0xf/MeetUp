﻿using MongoDB.Entities;

namespace SearchService.Models;

public class Item : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Location { get; set; }
    public string Author { get; set; }
    public string Visibility { get; set; }
    public List<string> Images { get; set; }
}
