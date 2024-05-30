var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

/*app.UseCors(builder =>
{
    builder.WithOrigins("http://10.0.2.2").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});
*/
app.UseRouting();

try
{
    DbInitalizer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.MapHub<ChatHub>("/chathub");

app.MapControllers();

app.Run();
