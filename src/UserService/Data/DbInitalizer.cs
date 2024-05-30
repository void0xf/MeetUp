using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using UserService.Models;

public class DbInitalizer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync(
            "UserInfoDb",
            MongoClientSettings.FromConnectionString(
                app.Configuration.GetConnectionString("MongoDbConnection")
            )
        );

        await DB.Index<UserInfo>().Key(x => x.Username, KeyType.Text).CreateAsync();

        var count = await DB.CountAsync<UserInfo>();

        if (count == 0)
        {
            Console.WriteLine("Not Data Found, Seeding data");
            var itemData = await File.ReadAllTextAsync("Data/ExampleData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var items = JsonSerializer.Deserialize<List<UserInfo>>(itemData, options);

            await DB.SaveAsync(items);
        }
    }
}
