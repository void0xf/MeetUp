using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class MeetEventUpdatedConsumer : IConsumer<MeetEventUpdated>
{
    private readonly IMapper _mapper;

    public MeetEventUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<MeetEventUpdated> context)
    {
        Console.WriteLine("Consuming Update Message", context.Message.Id);
        var item = _mapper.Map<Item>(context.Message);
        await DB.Update<Item>()
            .Match(me => me.ID == context.Message.Id)
            .ModifyWith(item)
            .ExecuteAsync();
    }
}
