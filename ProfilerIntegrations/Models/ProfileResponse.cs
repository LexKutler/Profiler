using ProfilerIntegrations.Entities;

namespace ProfilerIntegrations.Models;

/// <summary>
/// Safe representation of <see cref="UserProfile"/> for exposure to end-user
/// </summary>
public class ProfileResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public int CountryCode { get; set; }
}