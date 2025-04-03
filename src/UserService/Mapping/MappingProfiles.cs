using AutoMapper;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Map from DTOs to domain models
            CreateMap<UserInfoDto, UserInfo>();
            CreateMap<CreateUserDto, UserInfo>();
            CreateMap<UpdateUserDto, UserInfo>();
            
            // Map from domain models to DTOs
            CreateMap<UserInfo, UserResponseDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ID));
            CreateMap<UserInfo, UserInfoDto>();
        }
    }
} 