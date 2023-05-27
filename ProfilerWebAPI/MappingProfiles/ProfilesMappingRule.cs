using AutoMapper;
using ProfilerIntegration.Entities;
using ProfilerIntegration.Models;
using ProfilerIntegration.System;

namespace ProfilerWebAPI.MappingProfiles;

public class ProfilesMappingRule : Profile
{
    public ProfilesMappingRule()
    {
        CreateMap<ProfileRequestModel, UserProfile>()
            .ForMember(prof => prof.Id,
                opt => opt.Ignore())
            .ForMember(prof => prof.TimeStamp,
                opt => opt.Ignore());

        CreateMap<UserProfile, ProfileResponse>()
            .ForMember(resp => resp.Id,
                opt => opt.MapFrom(prof => prof.Id.ToString()));

        CreateMap<UserProfile, UserProfileRecord>();

        CreateMap<ProfileResponse, UserProfileRecord>();
    }
}