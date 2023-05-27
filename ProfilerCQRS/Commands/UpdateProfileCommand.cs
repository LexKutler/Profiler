using MediatR;
using MongoDB.Driver;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.Commands;

public class UpdateProfileCommand : IRequest<UpdateResult>
{
    public UserProfile UserProfile { get; set; }
}