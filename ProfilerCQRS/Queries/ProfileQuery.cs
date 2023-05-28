using MediatR;
using MongoDB.Bson;
using ProfilerIntegrations.Entities;

namespace ProfilerCQRS.Queries;

/// <summary>
/// Query object that implements <see cref="IRequest"/>
/// See <a href="https://github.com/jbogard/MediatR/wiki#basics">MediatR basics</a>
/// </summary>
/// <returns><see cref="UserProfile"/></returns>
public class ProfileQuery : IRequest<UserProfile>
{
    public ObjectId Id { get; set; }
}