using AutoMapper;
using EventService.DTOs;
using EventService.Models;

namespace EventService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<MeetEvent, MeetEventDto>();
        CreateMap<CreateMeetEventDto, MeetEvent>();
    }
}
