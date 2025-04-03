using EventService.Consumers;
using EventService.Data;
using EventService.Repositories;
using EventService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddDbContext<MeetEventDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register repositories and services
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService.Services.EventService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ConversationCreatedConsumer>();

    //outbox when incase message will not be delivered
    x.AddEntityFrameworkOutbox<MeetEventDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);

        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq(
        (context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        }
    );
});
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    DbInitalizer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}

app.Run();
