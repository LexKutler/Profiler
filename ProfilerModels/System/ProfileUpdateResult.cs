using ProfilerIntegration.Entities;

namespace ProfilerIntegration.System;
public class ProfileUpdateResult
{
    public UserProfile ProfileBefore { get; set; }
    public UserProfile ProfileAfter { get; set; }
}
