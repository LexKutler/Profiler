using MediatR;
using MongoDB.Bson;
using ProfilerIntegration.Entities;

namespace ProfilerCQRS.Queries;

public class ProfileQuery : IRequest<UserProfile>
{
    public ObjectId Id { get; set; }
}