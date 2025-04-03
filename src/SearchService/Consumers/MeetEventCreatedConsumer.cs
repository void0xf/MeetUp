using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Consumers;

public class MeetEventCreatedConsumer : IConsumer<MeetEventCreated>
{
    private readonly IMapper _mapper;
    private readonly ISearchService _searchService;

    public MeetEventCreatedConsumer(IMapper mapper, ISearchService searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task Consume(ConsumeContext<MeetEventCreated> context)
    {
        Console.WriteLine("Consuming 'Create' message", context.Message.Id);
        var item = _mapper.Map<Item>(context.Message);
        await _searchService.CreateItemAsync(item);
    }
}
