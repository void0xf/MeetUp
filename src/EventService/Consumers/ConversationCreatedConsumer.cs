using Contracts;
using EventService.Data;
using MassTransit;

namespace EventService.Consumers;

public class ConversationCreatedConsumer : IConsumer<ConversationCreated>
{
    private readonly MeetEventDbContext _dbContext;

    public ConversationCreatedConsumer(MeetEventDbContext context)
    {
        _dbContext = context;
    }

    public async Task Consume(ConsumeContext<ConversationCreated> context)
    {
        Console.WriteLine(
            $"Consuming ConversationCreated Message",
            context.Message.conversationId,
            context.Message.MeetEventId
        );
        var eventToUpdate = _dbContext.MeetEvents.FirstOrDefault(m =>
            m.Id.ToString() == context.Message.MeetEventId
        );
        if (eventToUpdate != null)
        {
            eventToUpdate.ConversationId = context.Message.conversationId;
            _dbContext.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Event To update not found");
        }
    }
}
