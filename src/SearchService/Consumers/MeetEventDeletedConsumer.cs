using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class MeetEventDeletedConsumer : IConsumer<MeetEventDeleted>
{
    private readonly IMapper _mapper;

    public MeetEventDeletedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<MeetEventDeleted> context)
    {
        Console.WriteLine("Consuming delete message ", context.Message.Id);
        await DB.DeleteAsync<Item>(context.Message.Id);
    }
}
