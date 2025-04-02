using ConversationService.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<MeetEventCreatedConsumer>();
    x.AddConsumersFromNamespaceContaining<MeetEventParticipantAddedConsumer>();

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

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

/*app.UseCors(builder =>
{
    builder.WithOrigins("http://10.0.2.2").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
});
*/
app.UseRouting();

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

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
