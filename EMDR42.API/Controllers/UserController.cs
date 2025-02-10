using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Templates;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Compilers;
using SqlKata.Execution;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IUserProfileRepository _profileService;
    private readonly IContactRepository _contactService;
    private readonly IQualificationRepository _qualificationService;
    private readonly ITherapyRepository _therapyRepository;

    public UserController(
        IEmailService emailService,
        IMapper mapper,
        IUserRepository userService,
        ILogger<UserController> logger,
        IUserProfileRepository profileService,
        IContactRepository contactService,
        IQualificationRepository qualificationService,
        ITherapyRepository therapyRepository)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
        _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        _qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
        _therapyRepository = therapyRepository ?? throw new ArgumentNullException(nameof(therapyRepository));
    }

    [HttpGet("confirm")]
    [SwaggerOperation(Summary = "Подтверждение почты пользователя")]
    public async Task<ActionResult> Get([FromQuery] string email)
    {
        var id = await _userService.GetUserIdAsync(email);

        if (id > 0)
        {
            try
            {
                _logger.LogInformation("Пользователь прошёл проверку");

                var resUserProfile = await _profileService.CreateAsync(new UserProfileModel { UserId = id });
                if (resUserProfile != 1) throw new Exception("Ошибка создания профиля пользователя");
                var resUserContact = await _contactService.CreateAsync(new ContactsModel { UserId = id, ContactEmail = email });
                if (resUserContact != 1) throw new Exception("Ошибка создания контактов пользователя");
                var resUserQualification = await _qualificationService.CreateAsync(new QualificationModel { UserId = id });
                if (resUserQualification != 1) throw new Exception("Ошибка создания данных квалификации пользователя");
                var resTherapy = await _therapyRepository.CreateAsync(new TherapyModel { UserId = id });
                if (resUserQualification != 1) throw new Exception("Ошибка создания данных о методах лечения");

                var result = await _userService.UserConfirmAsync(email);

                if (result != 1)
                {
                    _logger.LogError($"Произошла ошибка при подтверждении пользователя");
                    return NotFound(new ProblemDetails
                    {
                        Title = "NotFound",
                        Detail = "Произошла ошибка при подтверждении пользователя"
                    });
                }

                string htmlContent = ResponseTemplate.ConfirmResponse;
                return Content(htmlContent, "text/html");
            }
            catch (Exception ex)
            {

                await _profileService.DeleteAsync(id);
                await _contactService.DeleteAsync(id);
                await _qualificationService.DeleteAsync(id);
                await _therapyRepository.DeleteAsync(id);

                _logger.LogError(ex, "An error occurred while fetching clients.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
                });
            }
        }
        else
        {
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Неверный email адрес!"
            });
        }
    }

    [HttpPost("create")]
    [SwaggerOperation(Summary = "Создание нового пользователя в системе")]
    public async Task<ActionResult> Create([FromBody] LoginRequest req)
    {
        if (string.IsNullOrEmpty(req.Email))
        {
            _logger.LogError("Поле Email не заполнено");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Поле Email не заполнено"
            });
        }

        var findEmail = await _userService.CheckedUserByLoginAsync(req.Email);

        if (findEmail)
        {
            _logger.LogError("Пользователь с таким email уже существует");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Пользователь с таким email уже существует"
            });
        }

        var user = _mapper.Map<UserModel>(req);

        try
        {
            var resUser = await _userService.CreatedUserAsync(user);
            if (resUser != 1) throw new Exception("Ошибка создания пользователя");

            var person = new SendEmailDto() { Email = req.Email, Name = "", Subject = "Confirm email", MessageBody = EmailTemplates.RegistrationEmailTemplate.Replace("@email", req.Email) };
            _logger.LogInformation("Начало отправки сообщения пользователю");
            await _emailService.SendEmail(person);

            _logger.LogInformation("Сообщение отправлено пользователю");
            return Ok(req.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при создании пользователя: \n {ex.Message} \n {ex.StackTrace}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. {ex.Message}"
            });

        }
    }

    [HttpPost("change/password")]
    [SwaggerOperation(Summary = "Смена пароля пользователя")]
    public async Task<ActionResult> SendEmailForChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Salt))
        {
            _logger.LogError("Данные не заполнены");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Данные не заполнены"
            });
        }

        try
        {
            var result = await _userService.ChangePasswordAsync(request);

            if (result != 1)
            {
                _logger.LogError("Произошла ошибка при смене пароля, возможно ввели неправильные данные");
                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "Произошла ошибка при смене пароля, возможно ввели неправильные данные"
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Произошла ошибка при обработке запроса: \n {ex.Message} \n {ex.StackTrace}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal server error",
                Detail = $"Произошла ошибка при обработке запроса. {ex.Message}"
            });

        }
    }

    [HttpGet("change/email/{email}/{id}")]
    [SwaggerOperation(Summary = "Подтверждение почты пользователя")]
    public async Task<ActionResult> ChangeEmails([FromRoute] string email, [FromRoute] int id, [FromQuery] int contact)
    {
        if (string.IsNullOrEmpty(email) || id < 0)
        {
            _logger.LogError("Данные не заполнены");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Данные не заполнены"
            });
        }

        try
        {
            var result = await _userService.ChangeEmailAsync(id, email);

            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при обновлении адреса электронной почты");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при обновлении адреса электронной почты"
                });
            }

            if(contact == 1)
            {
                result = await _contactService.ChangeEmailAsync(id, email);

                if (result != 1)
                {
                    _logger.LogError($"Произошла ошибка при обновлении контактного адреса электронной почты");
                    return NotFound(new ProblemDetails
                    {
                        Title = "NotFound",
                        Detail = "Произошла ошибка при обновлении контактного адреса электронной почты"
                    });
                }
            }

            return Redirect("");
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

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удаление пользователя. Админ-панель")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result != 1)
            {
                _logger.LogError($"Произошла ошибка при удалении пользователя, возможно пользователь не найден: id = {id}");
                return NotFound(new ProblemDetails
                {
                    Title = "NotFound",
                    Detail = "Произошла ошибка при удалении пользователя, возможно пользователь не найден"
                });
            }
            await _profileService.DeleteAsync(id);
            await _contactService.DeleteAsync(id);
            await _qualificationService.DeleteAsync(id);
            await _therapyRepository.DeleteAsync(id);

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
