using MediatR;
using MongoDB.Driver;
using ProfilerCQRS.Commands;
using ProfilerIntegration.Entities;
using ProfilerModels;
using ProfilerModels.Abstractions;

namespace ProfilerCQRS.CommandHandlers;
public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand>
{
    private readonly IMongoCollection<Profile> _profiles;
    public CreateProfileCommandHandler(IMongoDBService mongoDbService)
    {
        _profiles = mongoDbService.Profiles;
    }
    public async Task Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        await _profiles.InsertOneAsync(request.Profile, cancellationToken: cancellationToken);
    }
}
