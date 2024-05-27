using System.Text.Json;
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
            Console.WriteLine("Not Data Found, Seeding data");
            var itemData = await File.ReadAllTextAsync("Data/ExampleData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
