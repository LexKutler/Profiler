using MongoDB.Bson;
using ProfilerIntegration.Entities;
using ProfilerIntegration.System;

namespace ProfilerModels.Abstractions;
public interface IProfileService
{
    Task<UserProfile> CreateProfileAsync(UserProfile userProfile);
    Task<ProfileUpdateResult> UpdateProfileAsync(UserProfile userProfile);
}