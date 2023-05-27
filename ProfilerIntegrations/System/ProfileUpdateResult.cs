using ProfilerIntegrations.Models;

namespace ProfilerIntegrations.System;

public class ProfileUpdateResult
{
    public ProfileResponse ProfileBefore { get; set; }
    public ProfileResponse ProfileAfter { get; set; }
}