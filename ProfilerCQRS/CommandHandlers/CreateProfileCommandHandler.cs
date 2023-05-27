using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.CommandHandlers;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand>
{
    private readonly IMongoCollection<UserProfile> _profiles;

    public CreateProfileCommandHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
    }

    public async Task Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        await _profiles.InsertOneAsync(request.UserProfile, cancellationToken: cancellationToken);
    }
}