using AutoMapper;
using Contracts;
using MassTransit;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Consumers;

public class MeetEventDeletedConsumer : IConsumer<MeetEventDeleted>
{
    private readonly IMapper _mapper;
    private readonly ISearchService _searchService;

    public MeetEventDeletedConsumer(IMapper mapper, ISearchService searchService)
    {
        _mapper = mapper;
        _searchService = searchService;
    }

    public async Task Consume(ConsumeContext<MeetEventDeleted> context)
    {
        Console.WriteLine("Consuming delete message ", context.Message.Id);
        await _searchService.DeleteItemAsync(context.Message.Id);
    }
}
