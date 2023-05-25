using MediatR;
using ProfilerIntegration.Entities;
using ProfilerModels;

namespace ProfilerCQRS.Commands;
public class CreateProfileCommand: IRequest
{
    public Profile Profile { get; set; }
}
