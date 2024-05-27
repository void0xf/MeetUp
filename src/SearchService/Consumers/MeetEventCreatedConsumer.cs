using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class MeetEventCreatedConsumer : IConsumer<MeetEventCreated>
{
    private readonly IMapper _mapper;

    public MeetEventCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<MeetEventCreated> context)
    {
        Console.WriteLine("Cosuming message", context.Message);
        var item = _mapper.Map<Item>(context.Message);
        await item.SaveAsync();
    }
}
