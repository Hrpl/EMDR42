using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/contacts")]
[ApiController]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactsController> _logger;
    public ContactsController(IContactRepository contactService, IMapper mapper, ILogger<ContactsController> logger)
    {
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    /// <summary>
    /// Получение контактов пользователя. Необходим JWT
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Получение контактов пользователя. Необходим JWT")]
    public async Task<ActionResult<ContactsDTO>> Get()
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

            var response = await _contactService.GetUserContactsAsync(Convert.ToInt32(userId));

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
    /// Обновление контактов пользователя. Необходим JWT
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [SwaggerOperation(Summary = "Обновление контактов пользователя. Необходим JWT")]
    public async Task<ActionResult> Put(ContactsDTO request)
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

            var model = _mapper.Map<ContactsModel>(request);
            model.UserId = Convert.ToInt32(userId);

            var result = await _contactService.UpdateUserContactsAsync(model);
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
