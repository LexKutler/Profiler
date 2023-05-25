using MediatR;
using ProfilerIntegration.Entities;
using ProfilerModels;

namespace ProfilerCQRS.Commands;
public class UpdateProfileCommand: IRequest<long>
{
    public Profile Profile { get; set; }
}
