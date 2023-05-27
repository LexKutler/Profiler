using MongoDB.Bson;
using ProfilerIntegration.Entities;
using ProfilerIntegration.System;

namespace ProfilerModels.Abstractions;

/// <summary>
/// Service provides CRU methods for profiles
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Basic find by <paramref name="profileId"/> operation
    /// </summary>
    /// <returns>Found profile or <see langword="null"/></returns>
    Task<UserProfile?> GetProfileByIdAsync(ObjectId profileId);
    /// <summary>
    /// Creates user profile
    /// </summary>
    /// <returns>Created profile found after operation</returns>
    Task<UserProfile> CreateProfileAsync(UserProfile userProfile);
    /// <summary>
    /// Patches user profile to <paramref name="userProfile"/>
    /// </summary>
    /// <param name="userProfile">Updated version of user profile<see cref="ObjectId"/></param>
    /// <returns>Pre and post version of profile</returns>
    Task<ProfileUpdateResult> UpdateProfileAsync(UserProfile userProfile);
}