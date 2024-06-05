using Contracts;
using ConversationService.Models;
using MassTransit;
using MongoDB.Entities;

namespace ConversationService.Consumers;

public class MeetEventCreatedConsumer : IConsumer<MeetEventCreated>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MeetEventCreatedConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<MeetEventCreated> context)
    {
        Console.WriteLine("Cosuming 'Create' message", context.Message.Id);

        var newConversation = new Conversation();
        newConversation.IsDM = false;
        newConversation.Participants = context.Message.Participants;
        await newConversation.SaveAsync();

        var conversationCreatedMessage = new ConversationCreated();
        conversationCreatedMessage.conversationId = newConversation.ID;
        conversationCreatedMessage.MeetEventId = context.Message.Id;

        await _publishEndpoint.Publish(conversationCreatedMessage);
    }
}
