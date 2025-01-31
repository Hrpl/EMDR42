using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("api/qualification")]
[ApiController]
[Authorize]
public class QualificationController : ControllerBase
{
    private readonly IQualificationService _qualificationService;
    private readonly IMapper _mapper;
    private readonly ILogger<QualificationController> _logger;
    public QualificationController(IQualificationService qualificationService, IMapper mapper, ILogger<QualificationController> logger)
    {
        _qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    /// <summary>
    /// Получение данных о квалификации пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получение данных о квалификации пользователя. Необходим JWT")]
    public async Task<ActionResult<QualificationDTO>> Get()
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

            var response = await _qualificationService.GetUserQualificationAsync(Convert.ToInt32(userId));

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
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Обновление данных о квалификации пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [SwaggerOperation(Summary = "Обновление данных о квалификации пользователя. Необходим JWT")]
    public async Task<ActionResult> Put([FromBody] QualificationDTO request)
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

            var model = _mapper.Map<QualificationModel>(request);
            model.UserId = Convert.ToInt32(userId);

            var result = await _qualificationService.UpdateUserQualificationAsync(model);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении данных о квалификации пользователя");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении данных о квалификации пользователя"
                });
            }
            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }
}
