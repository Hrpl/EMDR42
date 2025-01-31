using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/session")]
[ApiController]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISessionRepository _sessionService;
    private readonly ILogger<SessionController> _logger;
    public SessionController( ISessionRepository sessionService, IMapper mapper, ILogger<SessionController> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _logger = logger;
    }

    /// <summary>
    /// Получение логов сессии
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("log")]
    [SwaggerOperation(Summary = "Получение логов сессии. Необходим JWT")]
    public async Task<ActionResult<IEnumerable<SessionLogResponse>>> Get([FromBody] GetSessionLogs request)
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

            var result = await _sessionService.GetSessionLogs(request);

            if (result == null)
            {
                _logger.LogError("Ошибка при получении истории сессии");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = $"Ошибка при получении истории сессии"
                });
            }

            return Ok(result);
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
    /// Создание записи о сессии. Необходим JWT
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Создание записи о сессии. Необходим JWT")]
    public async Task<ActionResult> Post([FromBody] SessionDTO request)
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

            var model = _mapper.Map<SessionModel>(request);

            var result = await _sessionService.CreateSessionAsync(model);

            if (result != 1)
            {
                _logger.LogError("Ошибка при создании записи о сессии");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = $"Ошибка при создании записи о сессии"
                });
            }

            return Created();
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
