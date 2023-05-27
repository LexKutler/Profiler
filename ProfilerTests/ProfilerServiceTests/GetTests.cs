using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Moq;
using ProfilerBusiness;
using ProfilerCQRS.Queries;
using ProfilerIntegrations.Entities;

namespace TestsProfiler.ProfilerServiceTests;

public class GetTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;

    public GetTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task GetProfile_WithEmptyId_ThrowsArgumentException(UserProfile profile)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<ProfileQuery>(),
            It.IsAny<CancellationToken>()).Result)
            .Returns(profile);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetProfileByIdAsync(profile.Id));
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task CreateProfile_ReturnsProfile(UserProfile profile)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profile);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        var result = await service.CreateProfileAsync(profile);

        Assert.NotNull(result);
        Assert.Equal(result, profile);
    }

    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>()
        {
            // 1. Profile with empty id
            new object[]
            {
                new UserProfile()
                {
                    Id = ObjectId.Empty
                },
            },

            // 2. Profile with default timestamp
            new object[]
            {
                new UserProfile
                {
                    Id = ObjectId.GenerateNewId(),
                    FirstName = "null",
                    LastName = "null",
                    Email = "null",
                    UserName = "null",
                    Address = "null",
                    City = "null",
                    State = "null",
                    Zip = 0,
                    CountryCode = 0,
                    TimeStamp = null,
                    PicturePath = "null"
                }
            }
        };

        return allSeeds.Skip(index).Take(count);
    }
}