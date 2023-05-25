using AutoMapper;
using ProfilerIntegration.Entities;
using ProfilerIntegration.Models;

namespace ProfilerWebAPI.MappingProfiles;
public class ProfilesMappingRule: Profile
{
    public ProfilesMappingRule()
    {
        CreateMap<ProfileRequestModel, UserProfile>()
            .ForMember(prof => prof.Id,
                opt => opt.Ignore())
            .ForMember(prof => prof.TimeStamp,
                opt => opt.Ignore());

        CreateMap<UserProfile, ProfileCreatedResponse>();
    }
}
