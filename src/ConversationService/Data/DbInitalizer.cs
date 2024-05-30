using System.Text.Json;
using ConversationService.Models;
using MongoDB.Driver;
using MongoDB.Entities;

public class DbInitalizer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync(
            "ConversationsDb",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")
            )
        );

        await DB.Index<Conversation>().Key(x => x.ID, KeyType.Text).CreateAsync();

        var count = await DB.CountAsync<Conversation>();

        if (count == 0)
        {
            Console.WriteLine("Not Data Found, Seeding data");
            var itemData = await File.ReadAllTextAsync("Data/ExampleData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = JsonSerializer.Deserialize<List<Conversation>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
