using ConversationService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<MeetEventCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("conversation", false));

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            cfg.ReceiveEndpoint(
                "conversation-meet-event-created",
                e =>
                {
                    e.UseMessageRetry(r => r.Interval(5, 5));
                    e.ConfigureConsumer<MeetEventCreatedConsumer>(context);
                }
            );
            cfg.ConfigureEndpoints(context);
        }
    );
});

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
