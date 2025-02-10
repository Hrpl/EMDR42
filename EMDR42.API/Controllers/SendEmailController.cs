using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.DTO;
using EMDR42.Domain.Commons.Templates;
using EMDR42.Domain.Models;
using EMDR42.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EMDR42.API.Controllers;

[Route("send/email")]
[ApiController]
public class SendEmailController(IEmailService emailService,
        IMapper mapper,
        IUserRepository userService,
        ILogger<SendEmailController> logger) : ControllerBase
{
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUserRepository _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    private readonly ILogger<SendEmailController> _logger;

    /// <summary>
    /// Отправка письма для смены почты
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("change/email/send")]
    [SwaggerOperation(Summary = "Отправка письма для смены почты",
        Description = "Если contact = 1 - email установится как контактный, иначе 0")]
    public async Task<ActionResult> SendEmailForChangeEmailAddress([FromQuery] string email, [FromQuery] int contact)
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

        if (string.IsNullOrEmpty(email))
        {
            _logger.LogError("Поле Email не заполнено");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Поле Email не заполнено"
            });
        }

        try
        {
            var content = EmailTemplates.ChangeEmailAddressTemplate
                .Replace("@email", email)
                .Replace("@id", userId)
                .Replace("@contact", contact.ToString());

            var person = new SendEmailDto()
            {
                Email = email,
                Name = "",
                Subject = "Change email",
                MessageBody = content
            };

            _logger.LogInformation("Начало отправки сообщения пользователю");
            await _emailService.SendEmail(person);

            _logger.LogInformation("Сообщение отправлено пользователю");
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

    /// <summary>
    /// Отправка письма для смены пароля
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("password/send")]
    [SwaggerOperation(Summary = "Отправка письма для смены пароля")]
    public async Task<ActionResult> SendEmailForChangePassword([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogError("Email не заполнено");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Email не заполнено"
            });
        }

        var findEmail = await _userService.CheckedUserByLoginAsync(email);

        if (!findEmail)
        {
            _logger.LogError("Пользователь с таким email не существует");
            return BadRequest(new ProblemDetails
            {
                Title = "BadRequest",
                Detail = "Пользователь с таким email не существует"
            });
        }

        try
        {
            //будет ключом для смены пароля
            var salt = await _userService.GetSaltByEmail(email);

            var person = new SendEmailDto()
            {
                Email = email,
                Name = "",
                Subject = "Change password",
                MessageBody = EmailTemplates.ChangePasswordEmailTemplate.Replace("@salt", salt)
            };
            _logger.LogInformation("Начало отправки сообщения пользователю");
            await _emailService.SendEmail(person);

            _logger.LogInformation("Сообщение отправлено пользователю");
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
}
