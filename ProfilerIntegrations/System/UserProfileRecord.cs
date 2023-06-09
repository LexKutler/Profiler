﻿using ProfilerIntegrations.Entities;

namespace ProfilerIntegrations.System;

/// <summary>
/// Meant to be used to compare <see cref="UserProfile"/> instances
/// </summary>
public record UserProfileRecord
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zip { get; set; }
    public int CountryCode { get; set; }
    public string? PicturePath { get; set; }
}