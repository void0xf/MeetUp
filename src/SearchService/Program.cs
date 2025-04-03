using MassTransit;
using SearchService.Consumers;
using SearchService.Data;
using SearchService.Repositories;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register repositories and services
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<ISearchService, SearchService.Services.SearchService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<MeetEventCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<MeetEventUpdatedConsumer>();
    x.AddConsumersFromNamespaceContaining<MeetEventDeletedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            cfg.ReceiveEndpoint(
                "search-meet-event-created",
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

var app = builder.Build();

app.UseHttpsRedirection();

// Initialize database with shared seed data
await DbInitalizer.InitDb(app);

app.MapControllers();

app.Run();
