using Contracts;
using ConversationService.Models;
using MassTransit;
using MongoDB.Entities;

namespace ConversationService.Consumers;

public class MeetEventParticipantAddedConsumer : IConsumer<MeetEventParticipantAdded>
{
    public async Task Consume(ConsumeContext<MeetEventParticipantAdded> context)
    {
        Console.WriteLine("Consuming MeetEventParticipantAdded");
        var conversation = await DB.Find<Conversation>().OneAsync(context.Message.ConversationId);
        conversation.Participants.Add(context.Message.ParticipantUsername);
        await conversation.SaveAsync();
    }
}
