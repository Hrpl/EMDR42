﻿using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("api/userProfile")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IMapper _mapper;
    private readonly IUserProfileService _userProfileService;
    public UserProfileController(IUserProfileService userProfileService, IJwtHelper jwtHelper, IMapper mapper)
    {
        _userProfileService = userProfileService;
        _jwtHelper = jwtHelper;
        _mapper = mapper;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получение основной информации пользователя. Необходим JWT")]
    public async Task<ActionResult<GetUserProfileDTO>> Get()
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var response = await _userProfileService.GetUserProfilesAsync(id);

                if(response != null) return Ok();
                else return BadRequest();
            }
            else return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Обновление основной информации пользователя. Необходим JWT")]
    public async Task<ActionResult> Put([FromBody] GetUserProfileDTO request)
    {
        try
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var id = await _jwtHelper.DecodJwt(token);

                var model = _mapper.Map<UserProfileModel>(request);
                model.UserId = id;

                await _userProfileService.UpdateUserProfileAsync( model);
                return Ok();
            }
            else return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
