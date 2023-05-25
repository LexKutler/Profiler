namespace ProfilerModels.Abstractions;
public interface IProfileService
{
    Task CreateProfile(Profile profile);
    Task UpdateProfile(Profile profile);
}