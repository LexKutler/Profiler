using MediatR;
using MongoDB.Driver;
using ProfilerIntegration.Entities;
using ProfilerModels;

namespace ProfilerCQRS.Commands;
public class UpdateProfileCommand: IRequest<UpdateResult>
{
    public UserProfile UserProfile { get; set; }
}
