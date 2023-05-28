using ProfilerIntegrations.Models;

namespace ProfilerIntegrations.System;

/// <summary>
/// Contains before and after of update result
/// </summary>
public class ProfileUpdateResult
{
    public ProfileResponse ProfileBefore { get; set; }
    public ProfileResponse ProfileAfter { get; set; }
}