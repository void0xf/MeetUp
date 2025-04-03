using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Consumers;

public class MeetEventUpdatedConsumer : IConsumer<MeetEventUpdated>
{
    private readonly IMapper _mapper;
    private readonly ISearchService _searchService;

    public MeetEventUpdatedConsumer(IMapper mapper, ISearchService searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task Consume(ConsumeContext<MeetEventUpdated> context)
    {
        Console.WriteLine("Consuming Update Message", context.Message.Id);
        var item = _mapper.Map<Item>(context.Message);
        await _searchService.UpdateItemAsync(item);
    }
}
