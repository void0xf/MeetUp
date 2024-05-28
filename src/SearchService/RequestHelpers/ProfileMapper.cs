using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<MeetEventCreated, Item>();
        CreateMap<MeetEventDeleted, Item>();
        CreateMap<MeetEventUpdated, Item>();
    }
}
