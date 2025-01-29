using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("api/client")]
[ApiController]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IMapper _mapper;
    private readonly ILogger<ClientController> _logger;

    public ClientController(IMapper mapper, IClientService clientService, ILogger<ClientController> logger)
    {
        _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(clientService));
        _logger = logger;
    }

    /// <summary>
    /// Получение списка клиентов.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Получение списка клиентов. Необходим JWT")]
    public async Task<ActionResult<IEnumerable<ClientDTO>>> Get()
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


            //todo: вывод клиентов и данных о его сессиях
            return Ok();
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
    /// Получение данных одного клиента.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получение данных одного клиента. Необходим JWT")]
    public async Task<ActionResult<GetAnyClientDTO>> Get(int id)
    {
        try
        {
            var result = await _clientService.GetClientAsync(id);

            if (result != null)
            {
                _logger.LogInformation("Запрос на получение данных о пользователе выполнен успешно");
                return Ok(result);
            }
            else
            {
                _logger.LogError("При запросе данных произошла ошибка");
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "При запросе данных произошла ошибка"
                });
            }
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
    /// Создание нового клиентов.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Создание нового клиентов. Необходим JWT")]
    public async Task<ActionResult> Post([FromBody] ClientDTO request)
    {
        if (request == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Request body cannot be null."
            });
        }

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

            var model = _mapper.Map<ClientModel>(request);
            model.UserId = Convert.ToInt32(userId);

            await _clientService.CreateClientAsync(model);

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

    /// <summary>
    /// Обновление данных клиента.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Обновление данных клиента. Необходим JWT")]
    public async Task<ActionResult> Put(int clientId, [FromBody] UpdateClientDTO request)
    {
        if (clientId > 0 || request == null)
        {
            return BadRequest("Invalid request data.");
        }

        try
        {
            var model = _mapper.Map<UpdateClientModel>(request);
            var result = await _clientService.UpdateClientAsync(clientId, model);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении пользователя, возможно пользователь не найден: id = {clientId}");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении пользователя, возможно пользователь не найден"
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
    /// Архивация и разархивация клиента.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="isArchived"></param>
    /// <returns></returns>
    [HttpPatch("archive")]
    [SwaggerOperation(Summary = "Архивация и разархивация клиента. Описание обязательно к прочтению. Необходим JWT",
        Description = "Если isArchived = true, значит пользователь архивирован и его надо разархивировать, " +
        "если isArchived = false, пользователя надо архивировать")]
    public async Task<ActionResult> Archive(int clientId, bool isArchived)
    {
        if (clientId > 0)
        {
            return BadRequest("Invalid request data.");
        }

        try
        {
            var result = await _clientService.ArchiveClientAsync(clientId, isArchived);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при архивировании пользователя, возможно пользователь не найден: id = {clientId}");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при архивировании пользователя, возможно пользователь не найден"
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
    /// Удаление данных клиента.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    [HttpDelete("{clientId}")]
    [SwaggerOperation(Summary = "Удаление данных клиента. Необходим JWT")]
    public async Task<ActionResult> Delete(int clientId)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(clientId);
            if(result != 1)
            {
                _logger.LogError($"Произошла ошибка при удалении пользователя, возможно пользователь не найден: id = {clientId}");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при удалении пользователя, возможно пользователь не найден"
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching clients.");
            return StatusCode(500, new ProblemDetails { 
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });
        }
    }
}
