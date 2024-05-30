using ConversationService.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Entities;

public class ChatHub : Hub
{
    public async Task AddMessageToConversation(string conversationId, string sender, string content)
    {
        var conversation = await DB.Find<Conversation>()
            .MatchID(conversationId)
            .ExecuteFirstAsync();
        if (conversation != null)
        {
            var message = new Message { Sender = sender, Content = content };
            conversation.Messages.Add(message);
            await conversation.SaveAsync();
        }
    }

    public async Task<Conversation> GetConversationById(string conversationId)
    {
        return await DB.Find<Conversation>().OneAsync(conversationId);
    }

    public async Task SendMessage(string conversationId, string user, string message)
    {
        await AddMessageToConversation(conversationId, user, message);
        await Clients.All.SendAsync("ReceiveMessage", user, message, conversationId);
    }

    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }
}
