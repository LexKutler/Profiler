using AutoMapper;
using MediatR;
using MongoDB.Bson;
using Moq;
using ProfilerBusiness;
using ProfilerCQRS.Commands;
using ProfilerCQRS.Queries;
using ProfilerIntegration.Entities;

namespace TestsProfiler;

public class ProfilerServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;
    public ProfilerServiceTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task CreateProfile_WithEmptyId_GenerateNewId(UserProfile profile)
    {
        // Method should first create, then get profile
        var sequence = new MockSequence();
        _mediatrMock.InSequence(sequence)
            .Setup(mediator => mediator.Send(
            It.IsAny<CreateProfileCommand>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatrMock.InSequence(sequence)
            .Setup(mediator => mediator.Send(
                It.IsAny<GetProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profile);

        var service = new ProfileService(_mediatrMock.Object);
        var result = await service.CreateProfileAsync(profile);

        Assert.NotEqual(ObjectId.Empty, result.Id);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 2)]
    public async Task CreateProfile_WithoutTimeStamp_GenerateTimeStamp(UserProfile profile)
    {
        // Method should first create, then get profile
        var sequence = new MockSequence();
        _mediatrMock.InSequence(sequence)
            .Setup(mediator => mediator.Send(
                It.IsAny<CreateProfileCommand>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mediatrMock.InSequence(sequence)
            .Setup(mediator => mediator.Send(
                It.IsAny<GetProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profile);

        var service = new ProfileService(_mediatrMock.Object);
        var result = await service.CreateProfileAsync(profile);

        Assert.NotNull(result.TimeStamp);
        Assert.NotEqual(0, result.TimeStamp);
        Assert.True((DateTime.UtcNow.Ticks - result.TimeStamp) < TimeSpan.TicksPerMinute);
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
                new UserProfile()
            },

            // 2. Profile with timestamp equal to 0
            new object[]
            {
                new UserProfile()
                {
                    TimeStamp = 0
                }
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}