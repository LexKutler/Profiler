using AutoMapper;
using ProfilerIntegrations.Entities;
using ProfilerIntegrations.Models;
using ProfilerIntegrations.System;

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