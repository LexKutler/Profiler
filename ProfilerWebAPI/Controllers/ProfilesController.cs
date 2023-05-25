using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProfilerIntegration.Entities;
using ProfilerIntegration.Models;
using ProfilerIntegration.System;
using ProfilerModels.Abstractions;
using Serilog;

namespace ProfilerWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        public ProfilesController(IProfileService profileService, IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
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
        public async Task<IActionResult> Patch(ObjectId profileId, [FromBody] ProfileRequestModel model)
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

        [HttpPost("/upload")]

        public async Task<IActionResult> UploadUserPicture(IFormFile picture)
        {
            return Ok();
        }
    }
}
