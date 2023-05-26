using AutoMapper;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using ProfilerBusiness;
using ProfilerCQRS.Commands;
using ProfilerCQRS.Queries;
using ProfilerIntegration.Entities;
using ProfilerIntegration.System;

namespace TestsProfiler;

public class ProfilerServiceUpdateTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;
    public ProfilerServiceUpdateTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task UpdateProfile_WithEmptyId_ThrowsArgumentException(UserProfile profileBefore, UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<UpdateProfileCommand>(),
            It.IsAny<CancellationToken>()).Result)
            .Returns(It.IsAny<UpdateResult>());

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profileBefore)
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateProfileAsync(profileAfter));
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 2)]
    public async Task UpdateProfile_WithNullOrZeroTimeStamp_ThrowsArgumentException(UserProfile profileBefore, UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<UpdateProfileCommand>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(It.IsAny<UpdateResult>());

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profileBefore)
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateProfileAsync(profileAfter));
    }

    [Theory]
    [MemberData(nameof(GetData), 3, 1)]
    public async Task UpdateProfile_OnNonExistingDocument_ThrowsKeyNotFoundException(UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<UpdateProfileCommand>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(It.IsAny<UpdateResult>());

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Throws<KeyNotFoundException>()
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateProfileAsync(profileAfter));
    }

    [Theory]
    [MemberData(nameof(GetData), 4, 1)]
    public async Task UpdateProfile_WithIdenticalProfiles_ThrowsInvalidOperationException(UserProfile profileBefore, UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<UpdateProfileCommand>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(It.IsAny<UpdateResult>());

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profileBefore)
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UpdateProfileAsync(profileAfter));
    }

    [Theory]
    [MemberData(nameof(GetData), 5, 1)]
    public async Task UpdateProfile_WithoutChanges_ThrowsInvalidOperationException(UserProfile profileBefore, UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<UpdateProfileCommand>(),
                It.IsAny<CancellationToken>()).Result)
            .Throws<InvalidOperationException>();

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profileBefore)
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UpdateProfileAsync(profileAfter));
    }

    [Theory]
    [MemberData(nameof(GetData), 5, 1)]
    public async Task UpdateProfile_ReturnsBeforeAndAfter(UserProfile profileBefore, UserProfile profileAfter)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
                It.IsAny<UpdateProfileCommand>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(new UpdateResultAdapter());

        _mediatrMock.SetupSequence(mediator => mediator.Send(
                It.IsAny<ProfileQuery>(),
                It.IsAny<CancellationToken>()).Result)
            .Returns(profileBefore)
            .Returns(profileAfter);

        var service = new ProfileService(_mediatrMock.Object, _mapper);
        var updateResult = await service.UpdateProfileAsync(profileAfter);

        var beforeFromResult = _mapper.Map<UserProfileRecord>(updateResult.ProfileBefore);
        var afterFromResult = _mapper.Map<UserProfileRecord>(updateResult.ProfileAfter);
        var beforeFromData = _mapper.Map<UserProfileRecord>(profileBefore);
        var afterFromData = _mapper.Map<UserProfileRecord>(profileAfter);

        Assert.NotNull(updateResult);
        Assert.IsType<ProfileUpdateResult>(updateResult);
        Assert.Equal(beforeFromData, beforeFromResult);
        Assert.Equal(afterFromData, afterFromResult);
    }

    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>()
        {
            // 1. Profile with empty id
            new object[]
            {
                new UserProfile(),
                new UserProfile()
                {
                    Id = ObjectId.Empty
                }
            },

            // 2. Profile with null timestamp
            new object[]
            {
                new UserProfile(),
                new UserProfile()
                {
                    TimeStamp = null
                }
            },

            // 2. Profile with timestamp equal to 0
            new object[]
            {
                new UserProfile(),
                new UserProfile()
                {
                    TimeStamp = 0
                }
            },

            // 3. Update non-existing document
            new object[]
            {
                new UserProfile()
                {
                    Id = ObjectId.GenerateNewId(),
                    UserName = $"{Guid.NewGuid()}",
                    TimeStamp = 50
                },
            },

            // 4. Identical profiles
            new object[]
            {
                new UserProfile()
                {
                    Id = ObjectId.GenerateNewId(),
                    TimeStamp = 50
                },
                new UserProfile()
                {
                    Id = ObjectId.GenerateNewId(),
                    TimeStamp = 50
                }
            },

            // 5. Dummy profiles
            new object[]
            {
                new UserProfile()
                {
                    Id = ObjectId.GenerateNewId(),
                    UserName = $"{Guid.NewGuid()}",
                    TimeStamp = 50
                },
                new UserProfile()
                {
                    Id = ObjectId.GenerateNewId(),
                    UserName = $"{Guid.NewGuid()}",
                    TimeStamp = 50
                }
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}