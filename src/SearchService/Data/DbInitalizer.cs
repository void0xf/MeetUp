using AutoMapper;
using Contracts;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public class DbInitalizer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync(
            "SearchDb",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")
            )
        );

        await DB.Index<Item>()
            .Key(x => x.Title, KeyType.Text)
            .Key(x => x.Author, KeyType.Text)
            .Key(x => x.Description, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        if (count == 0)
        {
            Console.WriteLine("No data found in SearchService, seeding from shared data");
            await SeedDirectlyFromSharedData();
        }
    }

    private static async Task SeedDirectlyFromSharedData()
    {
        var seedEvents = SeedData.GetSeedEvents();
        var items = seedEvents.Select(e => new Item
        {
            ID = e.Id.ToString(),
            Title = e.Title,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            EventStartDate = e.EventStartDate,
            EventEndDate = e.EventEndDate,
            Location = e.Location,
            Author = e.Author,
            Visibility = e.Visibility,
            Images = e.Images
        }).ToList();

        await DB.SaveAsync(items);
        Console.WriteLine($"Seeded {items.Count} events to SearchService database directly from shared data");
    }
} 