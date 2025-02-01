﻿using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Request;
using EMDR42.Domain.Commons.Templates;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Compilers;
using SqlKata.Execution;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EMDR42.API.Controllers;

[Route("api/[controller]")]
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

    public UserController(
        IEmailService emailService,
        IMapper mapper,
        IUserRepository userService,
        ILogger<UserController> logger,
        IUserProfileRepository profileService,
        IContactRepository contactService,
        IQualificationRepository qualificationService)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
        _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        _qualificationService = qualificationService ?? throw new ArgumentNullException(nameof(qualificationService));
    }

    /// <summary>
    /// Подтверждение почты пользователя
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("confirm")]
    [SwaggerOperation(Summary = "Подтверждение почты пользователя")]
    public async Task<ActionResult> Get([FromQuery] string email)
    {
        try
        {
            var id = await _userService.GetUserIdAsync(email);

            if (id > 0)
            {
                _logger.LogInformation("Пользователь прошёл проверку");
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


                var resUserProfile = await _profileService.CreateUserProfileAsync(new UserProfileModel { UserId = id });
                if (resUserProfile == 1) throw new Exception();
                var resUserContact = await _contactService.CreateUserContactsAsync(new ContactsModel { UserId = id, ContactEmail = email });
                if (resUserContact == 1) throw new Exception();
                var resUserQualification = await _qualificationService.CreateUserQualificationAsync(new QualificationModel { UserId = id });
                if (resUserQualification == 1) throw new Exception();


                string htmlContent = ResponseTemplate.ConfirmResponse;
                return Content(htmlContent, "text/html");
            }
            else
            {
                await _profileService.DeleteUserProfileAsync(id);
                await _contactService.DeleteUserContactsAsync(id);
                await _qualificationService.DeleteUserQualificationAsync(id);

                return BadRequest(new ProblemDetails
                {
                    Title = "BadRequest",
                    Detail = "Неверный email адрес!"
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
    /// Создание нового пользователя в системе
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("create")]
    [SwaggerOperation(Summary = "Создание нового пользователя в системе")]
    public async Task<ActionResult> Create([FromBody] LoginRequest req)
    {
        if (req.Email == "")
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
            if (resUser == 1) throw new Exception();

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
                Detail = $"Произошла ошибка при обработке запроса. \n {ex.Message}"
            });

        }
    }
}
