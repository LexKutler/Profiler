using ProfilerIntegration.Entities;
using ProfilerIntegration.Models;

namespace ProfilerIntegration.System;

public class ProfileUpdateResult
{
    public ProfileResponse ProfileBefore { get; set; }
    public ProfileResponse ProfileAfter { get; set; }
}