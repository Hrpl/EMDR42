using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("profile")]
[ApiController]
[Authorize]
public class UserProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserProfileRepository _userProfileService;
    private readonly ILogger<UserProfileController> _logger;
    public UserProfileController(IUserProfileRepository userProfileService, IMapper mapper, ILogger<UserProfileController> logger)
    {
        _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    /// <summary>
    /// Получение основной информации пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Получение основной информации пользователя. Необходим JWT")]
    public async Task<ActionResult<GetUserProfileDTO>> Get()
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "Invalid user ID in token."
                });
            }

            var response = await _userProfileService.GetAsync(Convert.ToInt32(userId));

            if (response == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "При запросе данных произошла ошибка"
                });
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Обновление информации 'О себе'
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    [HttpPost("about")]
    [SwaggerOperation(Summary = "Обновление информации 'О себе'. Необходим JWT")]
    public async Task<ActionResult> UpdateAboutMe([FromBody] UpdateAboutMeRequest request)
    {
        if (string.IsNullOrEmpty(request.AboutMe))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Invalid input data."
            });
        }

        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Invalid user ID in token."
            });
        }

        try
        {
            var response = await _userProfileService.UpdateAboutMeAsync(request.AboutMe, Convert.ToInt32(userId));

            if (response != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении фотографии пользователя");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении фотографии пользователя"
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Обновление фотографии пользователя
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    [HttpPost("photo")]
    [SwaggerOperation(Summary = "Обновление фотографии пользователя. Необходим JWT")]
    public async Task<ActionResult> UpdatePhoto([FromQuery] string photo)
    {
        if (string.IsNullOrEmpty(photo))
        {
            return BadRequest(new ProblemDetails {
                    Title = "BadRequest",
                    Detail = "Invalid input data."
            });
        }

        var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Invalid user ID in token."
            });
        }

        try
        {
            var response = await _userProfileService.UpdatePhotoAsync(photo, Convert.ToInt32( userId));

            if (response != 1) 
            {
                _logger.LogError($"Произошла ошибка при обновлении фотографии пользователя");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении фотографии пользователя"
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }


    /// <summary>
    /// Обновление основной информации пользователя.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновление основной информации пользователя. Необходим JWT")]
    public async Task<ActionResult> Put([FromBody] GetUserProfileDTO request, [FromRoute] string id)
    {
        try
        {
            var userId = id ?? HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "Invalid user ID in token."
                });
            }

            var model = _mapper.Map<UserProfileModel>(request);
            model.UserId = Convert.ToInt32(userId);

            var result = await _userProfileService.UpdateAsync(model);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении контактов пользователя");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении контактов пользователя"
                });
            }
            return NoContent();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }
}
