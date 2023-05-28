using System.ComponentModel.DataAnnotations;
using ProfilerIntegrations.Entities;

namespace ProfilerIntegrations.Models;

/// <summary>
/// Safe representation of <see cref="UserProfile"/> for accepting data from end-user
/// </summary>
public class ProfileRequestModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public int CountryCode { get; set; }
}