using MediatR;
using ProfilerIntegration.Entities;

namespace ProfilerCQRS.Commands;

public class CreateProfileCommand : IRequest
{
    public UserProfile UserProfile { get; set; }
}