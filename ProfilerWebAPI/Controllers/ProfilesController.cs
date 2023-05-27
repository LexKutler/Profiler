using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProfilerIntegrations.Abstractions;
using ProfilerIntegrations.Entities;
using ProfilerIntegrations.Models;
using ProfilerIntegrations.System;

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

        [HttpGet("{profileId}")]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string profileId)
        {
            if (string.IsNullOrEmpty(profileId))
            {
                return BadRequest(profileId);
            }

            var profileObjectId = ObjectId.Parse(profileId);

            if (profileObjectId == ObjectId.Empty)
            {
                return BadRequest("Profile id is empty");
            }

            var profile = await _profileService.GetProfileByIdAsync(profileObjectId);

            var response = _mapper.Map<ProfileResponse>(profile);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
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

            var response = _mapper.Map<ProfileResponse>(createdProfile);

            return Created($"/profiles/{response.Id}", response);
        }

        [HttpPatch("{profileId}")]
        [ProducesResponseType(typeof(ProfileUpdateResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch([FromRoute] string profileId, [FromBody] ProfileRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileObjectId = ObjectId.Parse(profileId);

            if (profileObjectId == ObjectId.Empty)
            {
                return BadRequest("Profile id is empty");
            }

            var profile = _mapper.Map<UserProfile>(model);

            profile.Id = profileObjectId;

            var updateResult = await _profileService.UpdateProfileAsync(profile);

            return Ok(updateResult);
        }

        [HttpPost("{profileId}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadUserPicture([FromRoute] string profileId, [FromForm] IFormFile picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profileObjectId = ObjectId.Parse(profileId);

            if (profileObjectId == ObjectId.Empty)
            {
                return BadRequest("Profile id is invalid");
            }

            var profile = await _profileService.GetProfileByIdAsync(profileObjectId);

            if (profile is null)
            {
                return NotFound("Profile not found");
            }

            if (picture.Length <= 0) return BadRequest("Image is empty");

            var newFileName = ObjectId.GenerateNewId() + Path.GetExtension(picture.FileName);
            var filePath = Path.Combine(
                _targetPicturesPath,
                newFileName);

            if (!Directory.Exists(_targetPicturesPath))
            {
                Directory.CreateDirectory(_targetPicturesPath);
            }

            await using var stream = System.IO.File.Create(filePath);
            await picture.CopyToAsync(stream);

            // If file is saved, first remove previous picture
            if (!string.IsNullOrEmpty(profile.PicturePath))
            {
                System.IO.File.Delete(profile.PicturePath);
            }

            // Then update profile
            profile.PicturePath = filePath;
            await _profileService.UpdateProfileAsync(profile);

            return Ok();
        }
    }
}