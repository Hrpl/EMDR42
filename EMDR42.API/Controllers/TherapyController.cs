using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("api/therapy")]
[ApiController]
[Authorize]
public class TherapyController : ControllerBase
{
    private readonly ITherapyRepository _therapyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactsController> _logger;
    public TherapyController(ITherapyRepository therapyRepository, IMapper mapper, ILogger<ContactsController> logger)
    {
        _therapyRepository = therapyRepository ?? throw new ArgumentNullException(nameof(therapyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    /// <summary>
    /// Получение данных о лечении и услугах
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Получение данных о лечении и услугах")]
    public async Task<ActionResult<TherapyDTO>> Get(int id)
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

            var response = await _therapyRepository.GetUserTherapyAsync(id);

            if (response == null)
            {
                _logger.LogError("При запросе данных произошла ошибка");
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
    /// Обновление е данных о лечении и услугах
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновление данных о лечении и услугах")]
    public async Task<ActionResult> Put(TherapyDTO request)
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

            var model = _mapper.Map<TherapyModel>(request);
            model.UserId = Convert.ToInt32(userId);

            var result = await _therapyRepository.UpdateUserQualificationAsync(model);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении контактов пользователя");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении данных о лечении и услугах"
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
