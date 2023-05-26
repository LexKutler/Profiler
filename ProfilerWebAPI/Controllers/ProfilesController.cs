using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProfilerIntegration.Entities;
using ProfilerIntegration.Models;
using ProfilerIntegration.System;
using ProfilerModels.Abstractions;
using ProfilerWebAPI.ProfileIO;

namespace ProfilerWebAPI.Controllers
{
    [Route("profiles")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        private readonly string _targetPicturesPath;

        public ProfilesController(
            IProfileService profileService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _profileService = profileService;
            _mapper = mapper;
            _targetPicturesPath = configuration["TargetPicturesPath"]!;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] ProfileRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = _mapper.Map<UserProfile>(requestModel);

            if (profile.Id == ObjectId.Empty)
            {
                profile.Id = ObjectId.GenerateNewId();
            }

            var createdProfile = await _profileService.CreateProfileAsync(profile);

            return Created($"/profiles/{createdProfile.Id}", createdProfile);
        }

        [HttpPatch("{profileId}")]
        [ProducesResponseType(typeof(ProfileUpdateResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch([FromRoute] ObjectId profileId, [FromBody] ProfileRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (profileId == ObjectId.Empty)
            {
                return BadRequest("Profile id is invalid");
            }

            var profile = _mapper.Map<UserProfile>(model);

            var updateResult = await _profileService.UpdateProfileAsync(profile);

            return Ok(updateResult);
        }

        [HttpPost("{profileId}/picture")]
        [ProducesResponseType(typeof(ProfileUpdateResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadUserPicture(ObjectId profileId, [FromForm] ProfileImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.ProfileId == ObjectId.Empty || request.ProfileId != profileId)
            {
                return BadRequest("Profile id is invalid");
            }

            var profile = await _profileService.GetProfileByIdAsync(request.ProfileId);

            if (profile is null)
            {
                return NotFound("Profile not found");
            }

            if (request.Image.Length <= 0) return BadRequest("Image is empty");

            var filePath = Path.Combine(
                _targetPicturesPath,
                Path.GetRandomFileName());

            await using var stream = System.IO.File.Create(filePath);
            await request.Image.CopyToAsync(stream);

            // If file is saved, first remove previous picture
            if (!string.IsNullOrEmpty(profile.PicturePath))
            {
                System.IO.File.Delete(profile.PicturePath);
            }

            // Then update profile
            profile.PicturePath = filePath;
            var updateResult = await _profileService.UpdateProfileAsync(profile);

            return Ok(updateResult);
        }
    }
}